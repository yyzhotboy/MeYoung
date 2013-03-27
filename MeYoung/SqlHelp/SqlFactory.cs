using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;

namespace System.Data
{
    sealed class SqlFactory : IDBFactory
    {
        #region IDBFactory 成员

        public DbConnection CreateConnection(string connString)
        {
            return new SqlConnection(connString);

        }
        public DbCommand CreateCommand()
        {
            return new SqlCommand();
        }

        public DbDataAdapter CreateDataAdapter()
        {
            return new SqlDataAdapter();
        }

        public DbTransaction CreateTransaction(DbConnection conn)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public DbDataReader CreateDataReader(DbCommand cmd)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public DbParameter CreateParameter()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public DbParameter CreateParameter(string parameterName, DbType dbType, object value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public DbParameter CreateParameter(string parameterName, DbType dbType, int size, object value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion



        /// <summary>
        /// 构建添加sql语句和参数列表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="primaryKey">主键</param>
        /// <param name="model">需要添加的实体类</param>
        /// <param name="sql">返回添加sql语句</param>
        /// <returns>参数列表</returns>
        public DbParameter[] SetSql_Add(string tableName, string primaryKey, object model, ref string sql)
        {
            StringBuilder strSql = new StringBuilder("insert into " + tableName + "( ");
            StringBuilder strVal = new StringBuilder(" values(");
            List<SqlParameter> parameterList = new List<SqlParameter>();

            Type t = model.GetType();
            try
            {
                foreach (System.Reflection.PropertyInfo pinfo in t.GetProperties())
                {
                    string name = pinfo.Name;//属性名称 
                    Type type = pinfo.PropertyType;//属性类型
                    object val = pinfo.GetValue(model, null);//属性值
                    if (name.ToLower().Equals(primaryKey.ToLower()))//不添加主键
                        continue;
                    if (val != null && name != "Transaction_TableName" && name != "Transaction_PrimaryKey" && name != "Transaction_type")
                    {
                        strSql.Append(name + ",");
                        strVal.Append("@" + name + ",");
                        parameterList.Add(new SqlParameter("@" + name, val));
                    }
                }
                strSql.Remove(strSql.Length - 1, 1);
                strVal.Remove(strVal.Length - 1, 1);
                strSql.Append(")" + strVal.ToString() + ");select @@identity;");
                sql = strSql.ToString();
                return (DbParameter[])parameterList.ToArray();
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /// <summary>
        /// 构建修改sql语句和参数列表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="primaryKey">主键</param>
        /// <param name="model">需要修改的实体类</param>
        /// <param name="sql">返回修改sql语句</param>
        /// <returns>参数列表</returns>
        public DbParameter[] SetSql_Update(string tableName, string primaryKey, object model, ref string sql)
        {
            StringBuilder strSql = new StringBuilder("update " + tableName + " set ");
            List<SqlParameter> parameterList = new List<SqlParameter>();

            Type t = model.GetType();
            try
            {
                foreach (System.Reflection.PropertyInfo pinfo in t.GetProperties())
                {
                    string name = pinfo.Name;//属性名称 
                    Type type = pinfo.PropertyType;//属性类型
                    object val = pinfo.GetValue(model, null);//属性值
                    if (name.ToLower().Equals(primaryKey.ToLower()))//不修改主键
                    {
                        parameterList.Add(new SqlParameter("@" + primaryKey, val));
                        continue;
                    }
                    if (val != null && name != "Transaction_TableName" && name != "Transaction_PrimaryKey" && name != "Transaction_type")
                    {
                        //如果是Nullable类型，且val值为基类型最小值是，赋值为空
                        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            Type tt = type.GetGenericArguments()[0];
                            if (tt.Name.Equals("Decimal") && val.Equals(Decimal.MinValue))
                                val = DBNull.Value;
                            else if (tt.Name.Equals("DateTime") && val.Equals(DateTime.MinValue))
                                val = DBNull.Value;
                            else if (tt.Name.Equals("Double") && val.Equals(Double.MinValue))
                                val = DBNull.Value;
                            else if (tt.Name.Equals("Int16") && val.Equals(Int16.MinValue))
                                val = DBNull.Value;
                            else if (tt.Name.Equals("Int32") && val.Equals(Int32.MinValue))
                                val = DBNull.Value;
                            else if (tt.Name.Equals("Int64") && val.Equals(Int64.MinValue))
                                val = DBNull.Value;
                            else if (tt.Name.Equals("Single") && val.Equals(Single.MinValue))
                                val = DBNull.Value;
                            else if (tt.Name.Equals("DateTime") && val.Equals(UInt16.MinValue))
                                val = DBNull.Value;
                            else if (tt.Name.Equals("UInt32") && val.Equals(UInt32.MinValue))
                                val = DBNull.Value;
                            else if (tt.Name.Equals("UInt64") && val.Equals(UInt64.MinValue))
                                val = DBNull.Value;
                        }
                        strSql.Append(name + "=@" + name + ",");
                        parameterList.Add(new SqlParameter("@" + name, val));
                    }
                }
                strSql.Remove(strSql.Length - 1, 1);
                strSql.Append(" where " + primaryKey + "=@" + primaryKey);
                sql = strSql.ToString();
                return (DbParameter[])parameterList.ToArray();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 构建修改sql语句和参数列表
        /// </summary>
        /// <param name="tableName">表名</param>        
        /// <param name="model">需要修改的实体类</param>
        /// <param name="updateWhere">修改条件</param>
        /// <param name="sql">返回修改sql语句</param>
        /// <returns>参数列表</returns>
        public DbParameter[] SetSql_Update(string tableName,  object model, string updateWhere, ref string sql)
        {
            StringBuilder strSql = new StringBuilder("update " + tableName + " set ");
            List<SqlParameter> parameterList = new List<SqlParameter>();

            Type t = model.GetType();
            try
            {
                foreach (System.Reflection.PropertyInfo pinfo in t.GetProperties())
                {
                    string name = pinfo.Name;//属性名称 
                    Type type = pinfo.PropertyType;//属性类型
                    object val = pinfo.GetValue(model, null);//属性值
                    //if (name.ToLower().Equals(primaryKey.ToLower()))//不修改主键
                    //{
                    //    parameterList.Add(new SqlParameter("@" + primaryKey, val));
                    //    continue;
                    //}
                    if (val != null && name != "Transaction_TableName" && name != "Transaction_PrimaryKey" && name != "Transaction_type")
                    {
                        //如果是Nullable类型，且val值为基类型最小值是，赋值为空
                        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            Type tt = type.GetGenericArguments()[0];
                            if (tt.Name.Equals("Decimal") && val.Equals(Decimal.MinValue))
                                val = DBNull.Value;
                            else if (tt.Name.Equals("DateTime") && val.Equals(DateTime.MinValue))
                                val = DBNull.Value;
                            else if (tt.Name.Equals("Double") && val.Equals(Double.MinValue))
                                val = DBNull.Value;
                            else if (tt.Name.Equals("Int16") && val.Equals(Int16.MinValue))
                                val = DBNull.Value;
                            else if (tt.Name.Equals("Int32") && val.Equals(Int32.MinValue))
                                val = DBNull.Value;
                            else if (tt.Name.Equals("Int64") && val.Equals(Int64.MinValue))
                                val = DBNull.Value;
                            else if (tt.Name.Equals("Single") && val.Equals(Single.MinValue))
                                val = DBNull.Value;
                            else if (tt.Name.Equals("DateTime") && val.Equals(UInt16.MinValue))
                                val = DBNull.Value;
                            else if (tt.Name.Equals("UInt32") && val.Equals(UInt32.MinValue))
                                val = DBNull.Value;
                            else if (tt.Name.Equals("UInt64") && val.Equals(UInt64.MinValue))
                                val = DBNull.Value;
                        }
                        strSql.Append(name + "=@" + name + ",");
                        parameterList.Add(new SqlParameter("@" + name, val));
                    }
                }
                strSql.Remove(strSql.Length - 1, 1);
                strSql.Append(" where " + updateWhere);
                sql = strSql.ToString();
                return (DbParameter[])parameterList.ToArray();
            }
            catch (Exception e)
            {
                throw e;
            }
        }


    }
}
