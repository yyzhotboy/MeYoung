using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Data.Common;

namespace System.Data
{
    sealed class OleDbFactory : IDBFactory
    {
        #region IDBFactory 成员

        public DbConnection CreateConnection(string connString)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public DbCommand CreateCommand()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public DbDataAdapter CreateDataAdapter()
        {
            throw new Exception("The method or operation is not implemented.");
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



        #region IDBFactory 成员

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
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IDBFactory 成员


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
            throw new Exception("The method or operation is not implemented.");
        }


        /// <summary>
        /// 构建修改sql语句和参数列表
        /// </summary>
        /// <param name="tableName">表名</param>        
        /// <param name="model">需要修改的实体类</param>
        /// <param name="updateWhere">修改条件</param>
        /// <param name="sql">返回修改sql语句</param>
        /// <returns>参数列表</returns>
        public DbParameter[] SetSql_Update(string tableName, object model, string updateWhere, ref string sql)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
