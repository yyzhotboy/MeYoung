using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Data.SqlClient;


namespace System.Data
{
    public partial class SqlHelp
    {
        #region 字段
        private DBFactory dbFactory = DBFactory.NewDBFactory();//数据工厂
        private IDBFactory factory;

        private DbCommand cmd;
        private DbConnection conn;

        private string dbType;//数据库类型
        private string connString;//连接数据库字符串

        #endregion

        #region 属性

        /// <summary>
        /// 参数列表
        /// </summary>
        public DbParameterCollection Parameters
        {
            get{ return cmd.Parameters;}
            set 
            { 
                cmd.Parameters.Clear();
                if (value != null)
                {
                    foreach (DbParameter p in value)
                        cmd.Parameters.Add(p);
                }
            }
        }
        /// <summary>
        /// Sqlserver参数列表
        /// </summary>
        public SqlParameter[] SqlParameters
        {
            set
            {
                if (value != null)
                {
                    foreach (SqlParameter parameter in value)
                    {
                        if (parameter != null)
                        {
                            // 检查未分配值的输出参数,将其分配以DBNull.Value.
                            if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                                (parameter.Value == null))
                            {
                                parameter.Value = DBNull.Value;
                            }
                            cmd.Parameters.Add(parameter);
                        }
                    }
                }
            }
        }
        #region 注释掉内容
        /*
        /// <summary>
        /// 参数列表
        /// </summary>
        public DbParameter[] Parameters
        {
            get
            {
                DbParameter[] parameters = null;
                int length = cmd.Parameters.Count;
                if (length < 1)
                {
                    parameters = new DbParameter[length];
                    int index = 0;
                    foreach (DbParameter item in cmd.Parameters)
                    {
                        parameters[index] = item;
                        index++;
                    }
                }
                return parameters;

            }
            set
            {
                cmd.Parameters.Clear();
                if (value != null)
                {
                    foreach (DbParameter p in value)
                        cmd.Parameters.Add(p);
                }
            }
        }
        */
        #endregion

        /// <summary>
        /// 执行语句类型：存储过程 or Sql语句
        /// </summary>
        public CommandType CommandType
        {
            set
            {
                this.cmd.CommandType = value;
            }
            get
            {
                return this.cmd.CommandType;
            }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public SqlHelp()
        {
            dbType = ConfigurationManager.AppSettings["dbType"].ToLower().Trim();
            connString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString.Trim();
            this.OnLoad();//初始化各字段
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionStringsName">配置文件中，数据库连接字符串connectionStrings对应的name</param>
        public SqlHelp(string connectionStringsName)
        {
            dbType = ConfigurationManager.AppSettings["dbType"].ToLower().Trim();
            connString = ConfigurationManager.ConnectionStrings[connectionStringsName].ConnectionString.Trim();
            this.OnLoad();//初始化各字段
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="appSettingsKey_dbType">配置文件中，数据库类型AppSettings对应的key值</param>
        /// <param name="connectionStringsName">配置文件中，数据库连接字符串connectionStrings对应的name</param>
        public SqlHelp(string appSettingsKey_dbType, string connectionStringsName)
        {
            dbType = ConfigurationManager.AppSettings[appSettingsKey_dbType].ToLower().Trim();
            connString = ConfigurationManager.ConnectionStrings[connectionStringsName].ConnectionString.Trim();
            this.OnLoad();//初始化各字段
        }


        private void OnLoad()
        {
            factory = dbFactory.CreateFactory(dbType);//创建工厂实例
            conn = factory.CreateConnection(connString);//创建连接对象
            cmd = factory.CreateCommand();//创建操作对象

            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;//默认执行的是Sql语句
        }


        #endregion

        #region 事务处理
        /// <summary>
        /// 开始事务
        /// </summary>
        public void BeginTransaction()
        {

            if (cmd.Transaction == null)
            {

                this.OpenConnection();
                cmd.Transaction = conn.BeginTransaction();
            }
        }
        /// <summary>
        /// 回滚事务
        /// </summary>
        public void RollBack()
        {
            if (cmd.Transaction != null)
            {
                cmd.Transaction.Rollback();
            }
            this.CloseConnection();
        }
        /// <summary>
        /// 提交事务
        /// </summary>
        public void Commit()
        {
            if (cmd.Transaction != null)
            {
                cmd.Transaction.Commit();
            }
            this.CloseConnection();
        }
        /// <summary>
        /// 执行多个sql语句（带事务处理）
        /// </summary>
        /// <param name="sqlList"></param>
        public string TransactionSql(string[] sqlList)
        {
            this.BeginTransaction();
            try
            {
                foreach (string sql in sqlList)
                {
                    if (!string.IsNullOrEmpty(sql))
                    {

                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                    }
                }
                this.Commit();
            }
            catch (Exception e)
            {
                this.RollBack();
                //throw e;
                return e.Message;
            }
            return "";
        }

        /// <summary>
        /// 执行多个添加修改（带事务处理）
        /// </summary>
        /// <param name="modelList"></param>
        public string  TransactionModel(object[] modelList)
        {
            string retmsg = "";
            this.BeginTransaction();
            try
            {
                foreach (object models in modelList)
                {
                    Type t = models.GetType();
                    string Transaction_TableName = t.GetProperty("Transaction_TableName").GetValue(models, null).ToString();
                    string Transaction_PrimaryKey = t.GetProperty("Transaction_PrimaryKey").GetValue(models, null).ToString();
                    int Transaction_type = Convert.ToInt32(t.GetProperty("Transaction_type").GetValue(models, null).ToString());                    
                    //foreach (System.Reflection.PropertyInfo pinfo in t.GetProperties())
                    //{
                    //    string name = pinfo.Name;//属性名称 
                    //    object val = pinfo.GetValue(models, null);//属性值
                    //    if (name.Equals("Transaction_TableName"))//表名
                    //        Transaction_TableName = val.ToString();
                    //    if (name.Equals("Transaction_PrimaryKey"))//主键
                    //        Transaction_PrimaryKey = val.ToString();
                    //    if (name.Equals("Transaction_type"))//主键
                    //        Transaction_type = Convert.ToInt32(val.ToString());                       
                    //}
                    string sql = "";
                    DbParameter[] dp = null;
                    if(Transaction_type==1)
                    {
                        dp=factory.SetSql_Add(Transaction_TableName, Transaction_PrimaryKey, models, ref sql);
                    }
                    else
                    {
                        dp=factory.SetSql_Update(Transaction_TableName, Transaction_PrimaryKey, models, ref sql);
                    }
                    cmd.Parameters.Clear();
                    foreach (DbParameter item in dp)
                    {
                        //this.Parameters.Add(item);
                        cmd.Parameters.Add(item);
                    }
                    if (!string.IsNullOrEmpty(sql))
                    {
                        cmd.CommandText = sql;                       
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        retmsg = "没有进行任何修改！";
                        return retmsg;
                    }
                }
                this.Commit();
            }
            catch (Exception e)
            {
                this.RollBack();
                retmsg = e.Message;
                return retmsg;
            }
             return retmsg;
        }

        #endregion

        #region 打开连接对象
        /// <summary>
        /// 打开连接对象
        /// </summary>
        private void OpenConnection()
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
        }
        #endregion

        #region 关闭连接对象
        /// <summary>
        /// 关闭连接对象
        /// </summary>
        public void CloseConnection()
        {
            if (conn != null && cmd.Transaction == null)
            {

                conn.Close();
            }
        }
        #endregion

        /// <summary>
        /// 释放连接对象
        /// </summary>
        public void DiposeConnection()
        {
            conn.Dispose();
        }








    }
}
