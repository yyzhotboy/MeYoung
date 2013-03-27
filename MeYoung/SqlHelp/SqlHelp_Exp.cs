using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;

using System.Diagnostics;
using System.Data.SqlClient;

namespace System.Data
{
    public partial class SqlHelp
    {

        /// <summary>
        /// 获取某表某列最大值
        /// </summary>
        /// <param name="ColumnName">列名</param>
        /// <param name="TableName">表名</param>
        /// <returns></returns>
        public int GetMaxID(string ColumnName, string TableName)
        {
            string strsql = "select max(" + ColumnName + ") from " + TableName;
            object obj = ExecuteScalar(strsql);
            if (obj.ToString() == "")
            {
                return 1;
            }
            else
            {
                return int.Parse(obj.ToString());
            }
        }

        #region ExecuteNonQuery（执行增、删、改）
        /// <summary>
        /// 执行增、删、改
        /// </summary>
        /// <param name="sql">sql语句或存储过程</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql)
        {
            cmd.CommandText = sql;
            int result;
            try
            {
                this.OpenConnection();
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                cmd.Parameters.Clear();
                this.CloseConnection();
            }
            return result;
        }

        #endregion

        #region ExecuteScalar（执行单一查询）
        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。所有其他的列和行将被忽略。
        /// </summary>
        /// <param name="sql">sql语句或存储过程</param>
        /// <returns>返回结果</returns>
        public object ExecuteScalar(string sql)
        {
            object result = null;
            try
            {
                cmd.CommandText = sql;
                this.OpenConnection();
                result = cmd.ExecuteScalar();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Parameters.Clear();
                this.CloseConnection();
            }
            return result;
        }


        #endregion

        #region ExecuteReader（执行查询）
        /// <summary>
        /// 执行查询(需要显示关闭返回对象)
        /// </summary>
        /// <param name="sql">sql语句或存储过程</param>
        /// <returns>返回查询结果流</returns>
        public DbDataReader ExecuteReader(string sql)
        {
            DbDataReader result = null;
            try
            {
                cmd.CommandText = sql;
                this.OpenConnection();
                if (this.cmd.Transaction != null)
                    result = cmd.ExecuteReader();
                else
                    result = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception e)
            {
                this.CloseConnection();
                throw e;
            }
            return result;


        }


        #endregion

        #region ExecuteTable（执行查询）
        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="sql">sql语句或存储过程</param>
        /// <returns>返回一个DataTable</returns>
        public DataTable ExecuteTable(string sql)
        {
            DataTable result = new DataTable();
            try
            {
                cmd.CommandText = sql;
                DbDataAdapter adp = factory.CreateDataAdapter();
                adp.SelectCommand = cmd;
                adp.Fill(result);
            }
            catch (Exception e)
            {
                result = null;
                throw e;
            }
            finally
            {
                cmd.Parameters.Clear();
            }
            return result;
        }

        #endregion

        #region ExecuteDataSet（执行查询）
        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="sql">sql语句或存储过程</param>
        /// <returns>返回一个DataSet</returns>
        public DataSet ExecuteDataSet(string sql)
        {
            DataSet result = new DataSet();
            try
            {
                cmd.CommandText = sql;
                DbDataAdapter adp = factory.CreateDataAdapter();
                adp.SelectCommand = cmd;
                adp.Fill(result);
            }
            catch (Exception e)
            {
                result = null;
                throw e;
            }
            finally
            {
                cmd.Parameters.Clear();
            }
            return result;
        }

        #endregion


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="primaryKey">主键</param>
        /// <param name="model">添加的实体类</param>
        /// <returns></returns>
        public int Add(string tableName,string primaryKey, object model)
        {
            string sql = "";
            DbParameter[] dp = factory.SetSql_Add(tableName, primaryKey, model, ref sql);
            foreach(DbParameter item in dp)
            {
                this.Parameters.Add(item);
            }
            return int.Parse(this.ExecuteScalar(sql).ToString());
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="primaryKey">主键</param>
        /// <param name="model">修改的实体类</param>
        /// <returns></returns>
        public int Update(string tableName, string primaryKey, object model)
        {
            string sql = "";
            DbParameter[] dp = factory.SetSql_Update(tableName, primaryKey, model, ref sql);
            foreach (DbParameter item in dp)
            {
                this.Parameters.Add(item);
            }
            return this.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 构建修改sql语句和参数列表
        /// </summary>
        /// <param name="tableName">表名</param>        
        /// <param name="model">需要修改的实体类</param>
        /// <param name="updateWhere">修改条件</param>        
        /// <returns>参数列表</returns>        
        public int Update(string tableName, object model, string updateWhere)
        {
            string sql = "";
            DbParameter[] dp = factory.SetSql_Update(tableName, model, updateWhere, ref sql);
            foreach (DbParameter item in dp)
            {
                this.Parameters.Add(item);
            }
            return this.ExecuteNonQuery(sql);
        }



        #region 注释内容
        /*
        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
        public int ExecuteSqlTran(Hashtable SQLStringList)
        {
            int val = 0;
            using (cnn = new SqlConnection(strConnectionString))
            {
                cnn.Open();
                using (SqlTransaction trans = cnn.BeginTransaction())
                {
                    SqlCommand cmd = new SqlCommand();
                    try
                    {
                        //循环
                        foreach (DictionaryEntry myDE in SQLStringList)
                        {
                            string cmdText = myDE.Key.ToString();
                            SqlParameter[] cmdParms = (SqlParameter[])myDE.Value;
                            cmd.Connection = cnn;
                            cmd.CommandText = cmdText;
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.Text;

                            foreach (SqlParameter parameter in cmdParms)
                            {
                                if ((parameter.Direction == ParameterDirection.InputOutput ||
                                        parameter.Direction == ParameterDirection.Input) &&
                                        (parameter.Value == null))
                                {
                                    parameter.Value = DBNull.Value;
                                }

                                DbParameter dbp = (DbParameter)((ICloneable)parameter).Clone();
                                cmd.Parameters.Add(dbp);
                            }
                            //PrepareCommand(cmd, cnn, trans, cmdText, cmdParms);
                            val += cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                        return val;
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="sqlStringList"></param>
        /// <param name="prams"></param>
        /// <returns></returns>
        public bool ExecuteSqlTran(List<String> sqlStringList, List<SqlParameter[]> prams)
        {
            using (cnn = new SqlConnection(strConnectionString))
            {
                bool success = true;
                cnn.Open();
                SqlCommand comm = new SqlCommand();
                comm.Connection = cnn;
                SqlTransaction tx = cnn.BeginTransaction();
                comm.Transaction = tx;

                try
                {
                    for (int i = 0; i < sqlStringList.Count; i++)
                    {
                        //SqlCommand comm = new SqlCommand(sqlStringList[i],conn);
                        //comm.Transaction = tx;
                        comm.CommandText = sqlStringList[i];
                        if (prams.Count > 0)
                        {
                            foreach (SqlParameter p in prams[i])
                            {
                                //深层拷贝 防止错误
                                DbParameter dbp = (DbParameter)((ICloneable)p).Clone();
                                comm.Parameters.Add(dbp);
                            }
                        }
                        else
                        {
                            return false;
                        }
                        comm.ExecuteNonQuery();
                        comm.Parameters.Clear();
                    }
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    success = false;
                    tx.Rollback();
                }
                return success;
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="sqlStringList"></param>
        /// <returns></returns>
        public bool ExecuteSqlTran(List<String> sqlStringList)
        {
            using (cnn = new SqlConnection(strConnectionString))
            {
                bool success = true;
                cnn.Open();
                SqlCommand comm = new SqlCommand();
                comm.Connection = cnn;
                SqlTransaction tx = cnn.BeginTransaction();
                comm.Transaction = tx;

                try
                {
                    for (int i = 0; i < sqlStringList.Count; i++)
                    {
                        //SqlCommand comm = new SqlCommand(sqlStringList[i],conn);
                        //comm.Transaction = tx;
                        comm.CommandText = sqlStringList[i];

                        comm.ExecuteNonQuery();
                        comm.Parameters.Clear();
                    }
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    success = false;
                    tx.Rollback();
                }
                return success;
            }
        }

        /// <summary>
        /// 存储过程分页
        /// </summary>
        /// <param name="tableNameorviewName">表名或视图名</param>
        /// <param name="where">条件不必加Where 例:" id=3 and 等等" </param>
        /// <param name="orderName">排序字段名</param>
        /// <param name="desc">排序方式</param>
        /// <param name="pageSize">每页多少条数据</param>
        /// <param name="pageIndex">第几页</param>
        /// <returns>返回DataSet中包含2个DataTable Table[0]为分页查询数据 Table[1].Rows[0][0].ToString()为共多少数据</returns>
        public DataSet GetPageList(string tableNameorviewName, string where,  string orderName, string desc, int pageSize, int pageIndex)
        {
            
            SqlParameter[] parameters = {
											//表名 条件
											new SqlParameter("@tblName", tableNameorviewName),
											new SqlParameter("@fldName", "*"),
			                            	new SqlParameter("@strWhere", where),
											//-- 排序字段名 
			                            	new SqlParameter("@OrderfldName", orderName),
			                            	//页码
			                            	new SqlParameter("@PageSize", pageSize),
			                            	new SqlParameter("@PageIndex", pageIndex),
			                            	//返回记录总数, 非 0 值则返回
			                            	new SqlParameter("@IsReCount", 1),
			                            	//设置排序类型, 非 0 值则降序 
			                            	new SqlParameter("@OrderType", desc),
			                            	
			                            };

            return ExecuteDataSet("GetPageList", parameters);
        }
        */
        #endregion

        #region 无用
        /*

        /// <summary>
        /// 执行增、删、改
        /// </summary>
        /// <param name="sql">sql语句或存储过程</param>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql, params DbParameter[] parameters)
        {
            int result = 0;
            try
            {
                cmd.CommandText = sql;
                this.Parameters = parameters;
                this.OpenConnection();
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                this.CloseConnection();
            }
            return result;
        }


        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。所有其他的列和行将被忽略。
        /// </summary>
        /// <param name="sql">sql语句或存储过程</param>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        public Object ExecuteScalar(string sql, params DbParameter[] parameters)
        {
            Object result = null;
            try
            {
                cmd.CommandText = sql;

                this.Parameters = parameters;
                this.OpenConnection();
                result = cmd.ExecuteScalar();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                this.CloseConnection();
            }
            return result;
        }

        /// <summary>
        /// 执行查询(需要显示关闭返回对象)
        /// </summary>
        /// <param name="sql">sql语句或存储过程</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>返回查询结果流</returns>
        public DbDataReader ExecuteReader(string sql, params DbParameter[] parameters)
        {
            DbDataReader result = null;
            try
            {
                cmd.CommandText = sql;
                this.Parameters = parameters;
                this.OpenConnection();
                if (this.cmd.Transaction != null)
                    result = cmd.ExecuteReader();
                else
                    result = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception e)
            {
                this.CloseConnection();
                throw e;
            }
            return result;
        }
        
        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="sql">sql语句或存储过程</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>返回一个DataTable</returns>
        public DataTable ExecuteTable(string sql, params DbParameter[] parameters)
        {
            DataTable result = new DataTable();
            try
            {
                cmd.CommandText = sql;
                this.Parameters = parameters;
                DbDataAdapter adp = factory.CreateDataAdapter();
                adp.SelectCommand = cmd;
                adp.Fill(result);
            }
            catch (Exception e)
            {
                result = null;
                throw e;
            }
            return result;
        }

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="sql">sql语句或存储过程</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>返回一个DataSet</returns>
        public DataSet ExecuteDataSet(string sql, params DbParameter[] parameters)
        {
            DataSet result = new DataSet();
            try
            {
                cmd.CommandText = sql;
                this.Parameters = parameters;
                DbDataAdapter adp = factory.CreateDataAdapter();
                adp.SelectCommand = cmd;
                adp.Fill(result);
            }
            catch (Exception e)
            {
                result = null;
                throw e;
            }
            return result;
        }
        */
        #endregion



        #region SqlBulkCopyInsert 批量插入数据

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="dataTable">dataTable 格式的表数据</param>
        /// <param name="TableName">表名</param>
        /// <returns></returns>
        public long SqlBulkCopyInsert(DataTable dataTable,string TableName)
        {

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            //DataTable dataTable = GetTableSchema();
            //string passportKey;
            //for (int i = 0; i < count; i++)
            //{
            //    passportKey = Guid.NewGuid().ToString();
            //    DataRow dataRow = dataTable.NewRow();
            //    dataRow[0] = passportKey;
            //    dataTable.Rows.Add(dataRow);
            //}
            SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connString);
            sqlBulkCopy.DestinationTableName = TableName;
            sqlBulkCopy.BatchSize = dataTable.Rows.Count;
            SqlConnection sqlConnection = new SqlConnection(connString);
            sqlConnection.Open();
            if (dataTable != null && dataTable.Rows.Count != 0)
            {
                sqlBulkCopy.WriteToServer(dataTable);
            }
            sqlBulkCopy.Close();
            sqlConnection.Close();
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }

        #endregion

    }

}
