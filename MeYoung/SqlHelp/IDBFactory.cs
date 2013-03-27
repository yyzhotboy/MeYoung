using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

namespace System.Data
{
    interface IDBFactory
    {
        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <param name="strConn">连接字符串</param>
        /// <returns>Connection对象</returns>
        DbConnection CreateConnection(string connString);
        /// <summary>
        /// 建立Command对象
        /// </summary>
        /// <returns>Command对象</returns>
        DbCommand CreateCommand();
        /// <summary>
        /// 建立DataAdapter对象
        /// </summary>
        /// <returns>DataAdapter对象</returns>
        DbDataAdapter CreateDataAdapter();
        /// <summary>
        /// 根据Connection建立Transaction
        /// </summary>
        /// <param name="conn">Connection对象</param>
        /// <returns>Transaction对象</returns>
        DbTransaction CreateTransaction(DbConnection conn);
        /// <summary>
        /// 根据Command建立DataReader
        /// </summary>
        /// <param name="cmd">Command对象</param>
        /// <returns>DataReader对象</returns>
        DbDataReader CreateDataReader(DbCommand cmd);
        /// <summary>
        /// 建立Parameter对象
        /// </summary>
        /// <returns></returns>
        DbParameter CreateParameter();
        DbParameter CreateParameter(string parameterName, DbType dbType, object value);
        DbParameter CreateParameter(string parameterName, DbType dbType, int size, object value);

        /// <summary>
        /// 构建添加sql语句和参数列表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="primaryKey">主键</param>
        /// <param name="model">需要添加的实体类</param>
        /// <param name="sql">返回添加sql语句</param>
        /// <returns>参数列表</returns>
        DbParameter[] SetSql_Add(string tableName, string primaryKey, object model, ref string sql);

        /// <summary>
        /// 构建修改sql语句和参数列表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="primaryKey">主键</param>
        /// <param name="model">需要修改的实体类</param>
        /// <param name="sql">返回修改sql语句</param>
        /// <returns>参数列表</returns>
        DbParameter[] SetSql_Update(string tableName, string primaryKey, object model, ref string sql);


        /// <summary>
        /// 构建修改sql语句和参数列表
        /// </summary>
        /// <param name="tableName">表名</param>        
        /// <param name="model">需要修改的实体类</param>
        /// <param name="updateWhere">修改条件</param>
        /// <param name="sql">返回修改sql语句</param>
        /// <returns>参数列表</returns>
        DbParameter[] SetSql_Update(string tableName, object model, string updateWhere, ref string sql);

    }
}
