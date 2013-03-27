using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MeYoung.IDAL
{
    public interface IDBCommon
    {
        #region 通用方法
        /// <summary>
        /// 参数列表(执行需要参数化的方法时可使用,比如'where')
        /// </summary>
        /// <param name="param"></param>
        Dictionary<string, object> Parameters
        {
            set;
        }

        #region 通用（增、删、改）方法
        /// <summary>
        /// 通用（增、删、改）方法
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        int ExecuteNonQuery(string sql);
        /// <summary>
        /// 单查一行。
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        object ExecuteScalar(string sql);

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="dataTable">dataTable 格式的表数据</param>
        /// <param name="TableName">表名</param>
        /// <returns></returns>
        long SqlBulkCopyInsert(DataTable dataTable, string TableName);
        #endregion

        #region 通用查询方法
        /// <summary>
        /// 通用查询方法
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        DataSet ExecuteDataSet(string sql);
        /// <summary>
        /// 通用查询方法
        /// </summary>
        /// <param name="tableName">表名或视图名</param>
        /// <param name="where">查询条件(不包括'where')</param>
        /// <returns></returns>
        DataTable ExecuteDataTable(string tableName, string where);
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
        DataSet GetPageList(string tableName, string where, string sort, int pageSize, int pageIndex, bool isBackTotal, bool isBackPageCount);


        /// <summary>
        /// 返回符合条件的记录数
        /// </summary>
        /// <param name="tableName">表名或视图名</param>
        /// <param name="where">条件,可为空</param>
        /// <returns></returns>
        int GetCount(string tableName, string where);

        #endregion

        #region 事物处理（执行多个添加修改）

        /// <summary>
        /// 事物处理（执行多个添加修改）
        /// </summary>
        /// <param name="modelList">model列表</param>
        string TransactionModel(object[] modelList);
        /// <summary>
        /// 事物处理（执行多个添加修改）
        /// </summary>
        /// <param name="modelList">sql语句l列表</param>
        string TransactionSql(string[] sqlList);
        #endregion
        /// <summary>
        /// 判断是否子表是否有数据
        /// </summary>
        /// <param name="ProgramCode"></param>
        /// <returns></returns>
        string checkIsNoMeg(string ProgramCode);

        #endregion

        DataSet GetDictionary(int PID);
    }
}
