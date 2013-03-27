using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MeYoung.BLL
{
    public class DBCommonBLL
    {
           #region Must Have
        private IDAL.IDBCommon dal =new  MeYoung.DAL.DBCommonDAL();

        public DBCommonBLL() { }
        public DBCommonBLL(Dictionary<string, object> parameters)
        {
            dal.Parameters = parameters;
        }
        public Dictionary<string, object> Parameters
        {
            set { dal.Parameters = value; }
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
            return dal.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="dataTable">dataTable 格式的表数据</param>
        /// <param name="TableName">表名</param>
        /// <returns>返回执行的毫秒数</returns>
        public long SqlBulkCopyInsert(DataTable dataTable, string TableName)
        {
            return dal.SqlBulkCopyInsert(dataTable, TableName);
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
            return dal.ExecuteDataSet(sql);
        }
        /// <summary>
        /// 通用查询方法
        /// </summary>
        /// <param name="tableName">表名或视图名</param>
        /// <param name="where">查询条件(不包括'where')</param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string tableName, string where)
        {
            return dal.ExecuteDataTable(tableName, where);
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
            return dal.GetPageList(tableName, where, sort, pageSize, pageIndex, isBackTotal, isBackPageCount);
        }

        /// <summary>
        /// 返回符合条件的记录数
        /// </summary>
        /// <param name="tableName">表名或视图名</param>
        /// <param name="where">条件,可为空</param>
        /// <returns></returns>
        public int GetCount(string tableName, string where)
        {
            return dal.GetCount(tableName, where);
        }

        #endregion

        #region 事物处理（执行多个添加修改）

        /// <summary>
        /// 事物处理（执行多个添加修改model）
        /// </summary>
        /// <param name="modelList">model列表</param>
        public string TransactionModel(object[] modelList)
        {
            return dal.TransactionModel(modelList);
        }
        /// <summary>
        /// 事物处理（执行多个添加修改sql）
        /// </summary>
        /// <param name="modelList">sql语句l列表</param>
        public string TransactionSql(string[] sqlList)
        {
            return dal.TransactionSql(sqlList);
        }
        #endregion

        public string checkIsNoMeg(string ProgramCode)
        {
            return dal.checkIsNoMeg(ProgramCode);
        }
        /// <summary>
        /// 根据父ID得到字典表
        /// </summary>
        /// <param name="PID">父ID</param>
        /// <returns></returns>
        public DataSet GetDictionary(int PID)
        {
            return dal.GetDictionary(PID);
        }
    }
}
