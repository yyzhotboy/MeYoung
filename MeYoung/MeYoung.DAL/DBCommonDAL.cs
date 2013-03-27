using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.Transaction;
using System.Linq;
using System.Text;

namespace MeYoung.DAL
{
    public class DBCommonDAL:MeYoung.IDAL.IDBCommon
    {
                #region Must have
        private SqlHelp dbHelper;

        /// <summary>
        /// 参数列表(执行需要参数化的方法时可使用,比如'where')
        /// </summary>
        public Dictionary<string, object> Parameters
        {
            set
            {
                //int i = 0;
                this.dbHelper.Parameters.Clear();
                foreach (KeyValuePair<string, object> item in value)
                {
                    SqlParameter sparam = new SqlParameter(item.Key, item.Value);
                    this.dbHelper.Parameters.Add(sparam);
                }
            }

        }

        public DBCommonDAL()
        {
            this.dbHelper = new SqlHelp("dbType", "connString");
        }
        public DBCommonDAL(Transaction trans)
        {
            this.dbHelper = trans.GetSqlHelp();
        }
        #endregion

        #region 通用（增、删、改）方法
        /// <summary>
        /// 通用（增、删、改）方法
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql)
        {
            return dbHelper.ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 单独查询一行。
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public object ExecuteScalar(string sql)
        {
            return dbHelper.ExecuteScalar(sql);
        }
        #endregion

        #region 通用查询方法
        /// <summary>
        /// 通用查询方法
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string sql)
        {
            return dbHelper.ExecuteDataSet(sql);
        }
        /// <summary>
        /// 通用查询方法
        /// </summary>
        /// <param name="tableName">表名或视图名</param>
        /// <param name="where">查询条件(不包括'where')</param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string tableName, string where)
        {
            string sql = "select * from " + tableName;
            if (!string.IsNullOrEmpty(where))
                sql += " where 1=1 and " + where;
            return dbHelper.ExecuteTable(sql);
        }
        #endregion

        #region 分页查询
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="tableName">表名或视图名</param>
        /// <param name="where">查询条件---可为空   (不包括'where')</param>        
        /// <param name="sort">排序条件----必填      (不包含'order by')</param>  
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="isBackTotal">是否返回总记录数(true:是，false:否)</param>  
        /// <param name="isBackPageCount">是否返回总页数(true:是，false:否)</param>
        /// <returns></returns>
        public DataSet GetPageList(string tableName, string where, string sort, int pageSize, int pageIndex, bool isBackTotal, bool isBackPageCount)
        {
            DataSet ds = null;
            if (dbHelper.Parameters != null && dbHelper.Parameters.Count > 0)//有参数时，执行参数化查询
            {
                PageSql query = new PageSql(tableName, where, sort, pageIndex, pageSize);
                string sql = "";
                if (isBackTotal && !isBackPageCount)
                    sql = query.GetPage_Count_Sql();
                else if (!isBackTotal && isBackPageCount)
                    sql = query.GetPage_PageCount_Sql();
                else if (isBackTotal && isBackPageCount)
                    sql = query.GetPage_Count_PageCount_Sql();
                else if (!isBackTotal && !isBackPageCount)
                    sql = query.GetPageSql();
                ds = dbHelper.ExecuteDataSet(sql);

            }
            else//无参数时，执行存储过程
            {
                if (string.IsNullOrEmpty(where))
                    where = " 1=1";
                System.Data.Common.DbParameter[] paramList =
                    {
                        new SqlParameter("@TableName",SqlDbType.NVarChar),
                        new SqlParameter("@BackName",SqlDbType.NVarChar),
                        new SqlParameter("@PageSize",SqlDbType.Int),                       
                        new SqlParameter("@PageIndex",SqlDbType.Int),

                        new SqlParameter("@IsBackTotal",SqlDbType.Bit),
                        new SqlParameter("@IsBackPageCount",SqlDbType.Bit),
                        new SqlParameter("@Sort",SqlDbType.NVarChar),
                        new SqlParameter("@strWhere",SqlDbType.NVarChar),
                        new SqlParameter("@Group",SqlDbType.NVarChar)
                        
                
                    };
                paramList[0].Value = tableName;//表名或视图名
                paramList[1].Value = "*";//返回列
                paramList[2].Value = pageSize;
                paramList[3].Value = pageIndex;
                paramList[4].Value = isBackTotal ? 1 : 0;//如果是1，执行统计，然后返回总数结果
                paramList[5].Value = isBackPageCount ? 1 : 0;//如果是1，返回总页数记录
                paramList[6].Value = sort;//排序条件
                paramList[7].Value = where;
                paramList[8].Value = "";
                foreach (SqlParameter item in paramList)
                {
                    dbHelper.Parameters.Add(item);
                }
                dbHelper.CommandType = CommandType.StoredProcedure;
                ds = dbHelper.ExecuteDataSet("GetPage");

                dbHelper.CommandType = CommandType.Text;
            }
            return ds;
        }

        public string checkIsNoMeg(string PurchaseCID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select count(*) from CivilEngineeringContractDetail ");
            sb.Append("where PurchaseCID='" + PurchaseCID + "'");
            return dbHelper.ExecuteScalar(sb.ToString()).ToString();
        }
        /// <summary>
        /// 返回符合条件的记录数
        /// </summary>
        /// <param name="tableName">表名或视图名</param>
        /// <param name="where">条件,可为空</param>
        /// <returns></returns>
        public int GetCount(string tableName, string where)
        {
            string sql = "select count(0) from " + tableName;
            if (!string.IsNullOrEmpty(where))
                sql += " where " + where;
            return Convert.ToInt32(dbHelper.ExecuteScalar(sql));
        }


        #endregion

        #region 批量插入数据
        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="dataTable">dataTable 格式的表数据</param>
        /// <param name="TableName">表名</param>
        /// <returns></returns>
        public long SqlBulkCopyInsert(DataTable dataTable, string TableName)
        {
            return dbHelper.SqlBulkCopyInsert(dataTable, TableName);
        }
        //public object ExecuteScalar(DataTable dt)
        //{
        //    //构造一个Datatable存储将要批量导入的数据
        //    //DataTable dt = new DataTable();
        //    //dt.Columns.Add("id", typeof(string));
        //    //dt.Columns.Add("name", typeof(string));

        //    //// 见识下SqlBulkCopy强悍之处，来个十万条数数据试验
        //    //int i;
        //    //for (i = 0; i < 100000; i++)
        //    //{
        //    //    DataRow dr = dt.NewRow();
        //    //    dr["name"] = i.ToString();
        //    //    dt.Rows.Add(dr);
        //    //}

        //    string str = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString.ToString();
        //    //声明数据库连接
        //    SqlConnection conn = new SqlConnection(str);

        //    conn.Open();
        //    //声明SqlBulkCopy ,using释放非托管资源
        //    using (SqlBulkCopy sqlBC = new SqlBulkCopy(conn))
        //    {
        //        //一次批量的插入的数据量
        //        sqlBC.BatchSize = 1000;
        //        //超时之前操作完成所允许的秒数，如果超时则事务不会提交 ，数据将回滚，所有已复制的行都会从目标表中移除
        //        sqlBC.BulkCopyTimeout = 60;

        //        //設定 NotifyAfter 属性，以便在每插入10000 条数据时，呼叫相应事件。  
        //        sqlBC.NotifyAfter = 10000;
        //        sqlBC.SqlRowsCopied += new SqlRowsCopiedEventHandler(OnSqlRowsCopied);

        //        //设置要批量写入的表
        //        sqlBC.DestinationTableName = "dbo.text";

        //        //自定义的datatable和数据库的字段进行对应
        //        sqlBC.ColumnMappings.Add("id", "tel");
        //        sqlBC.ColumnMappings.Add("name", "neirong");

        //        //批量写入
        //        sqlBC.WriteToServer(dt);
        //    }
        //    conn.Dispose();
        //}
        #endregion

        #region 事物处理（执行多个添加修改）

        /// <summary>
        /// 事物处理（执行多个添加修改）
        /// </summary>
        /// <param name="modelList">model列表</param>
        public string TransactionModel(object[] modelList)
        {
            return dbHelper.TransactionModel(modelList);
        }
        /// <summary>
        /// 事物处理（执行多个添加修改）
        /// </summary>
        /// <param name="modelList">sql语句l列表</param>
        public string TransactionSql(string[] sqlList)
        {
            return dbHelper.TransactionSql(sqlList);
        }
        #endregion


        #region IT_Common 成员

        /// <summary>
        /// 根据父ID得到字典表数据
        /// </summary>
        /// <param name="PID">字典表父ID</param>
        /// <returns></returns>
        public DataSet GetDictionary(int PID)
        {
            string sql = "select * from Dictionary where ParentCode=" + PID;
            return dbHelper.ExecuteDataSet(sql);
        }
        #endregion
    }
}
