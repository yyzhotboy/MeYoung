using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace System.Data
{
    /// <summary>
    /// 分页语句类
    /// </summary>
    public class PageSql
    {
        private int _pageIndex = 1;//当前页索引(默认第一页)
        private int _pageSize = 10;//每页显示条数(默认10条)
        private string _pk; //主键
        private string _tableName;//表名或视图名
        private string _group;//分组条件
        private string _selectColumn = "*";//查询列(默认所有列)
        private string _sort;//排序条件(默认主键降序)
        private string _where;//查询条件        

                

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tableName">表名或视图名</param>        
        /// <param name="where">查询条件,可为空(不包括'where')</param>
        /// <param name="sort">排序条件,必填(不包含'order by')</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="pageSize">每页显示条数</param>
        public PageSql(string tableName, string where, string sort, int pageIndex, int pageSize)
        {
            this._tableName = tableName;
            //this._pk = pk;
            this._where = string.IsNullOrEmpty(where.Trim()) ? "1=1" : where;
            //if (string.IsNullOrEmpty(sort))
            //this._sort = _pk + " desc";
            //else
            this._sort = sort;
            this._pageIndex = pageIndex;
            this._pageSize = pageSize;
        }

        /// <summary>
        /// 主键,默认排序列(无主键时，可为表中随便一个列名)
        /// </summary>
        public string PK
        {
            get { return _pk; }
            set { _pk = value; }
        }
        /// <summary>
        /// 查询列(默认'*'表示全部)
        /// </summary>
        public string SelectColumn
        {
            get { return _selectColumn; }
            set { _selectColumn = value; }
        }

        /// <summary>
        /// 表名或视图名
        /// </summary>
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }
        /// <summary>
        /// 查询条件(不包括'where')
        /// </summary>
        public string Where
        {
            get { return _where; }
            set { _where = value; }
        }
        /// <summary>
        /// 分组条件(不包括'group by')
        /// </summary>
        public string Group
        {
            get { return _group; }
            set { _group = value; }
        }
        /// <summary>
        /// 排序条件(不包含'order by')
        /// </summary>
        public string Sort
        {
            get { return _sort; }
            set { _sort = value; }
        }

        /// <summary>
        /// 当前页数
        /// </summary>
        public int PageIndex
        {
            get { return _pageIndex; }
            set { _pageIndex = value; }
        }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }

        /// <summary>
        /// 生成查询记录总数的SQL语句
        /// </summary>
        /// <returns></returns>
        public string GetCountSql()
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select count(0) from {0}", TableName);
            if (!string.IsNullOrEmpty(Where))
                sql.AppendFormat(" where {0}", Where);
            if (!string.IsNullOrEmpty(Group))
                sql.AppendFormat(" group by {0}", Group);
            return sql.ToString();
        }
        /// <summary>
        /// 生成查询总页数的SQL语句
        /// </summary>
        /// <returns></returns>
        public string GetPageCountSql()
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select (count(0)/" + PageSize + ")+1 from {0}", TableName);
            if (!string.IsNullOrEmpty(Where))
                sql.AppendFormat(" where {0}", Where);
            if (!string.IsNullOrEmpty(Group))
                sql.AppendFormat(" group by {0}", Group);
            return sql.ToString();
        }


        /// <summary>
        /// 生成分页查询语句(包含记录总数)
        /// </summary>
        /// <returns></returns>
        public string GetPage_Count_Sql()
        {
            string sql1 = GetPageSql();
            string sql2 = GetCountSql();
            return sql1 + ";" + sql2;
        }
        /// <summary>
        /// 生成分页查询语句(包含总页数)
        /// </summary>
        /// <returns></returns>
        public string GetPage_PageCount_Sql()
        {
            string sql1 = GetPageSql();
            string sql2 = GetPageCountSql();
            return sql1 + ";" + sql2;
        }
        /// <summary>
        /// 生成分页查询语句(包含总数、总页数)
        /// </summary>
        /// <returns></returns>
        public string GetPage_Count_PageCount_Sql()
        {
            string sql1 = GetPageSql();
            string sql2 = GetCountSql();
            string sql3 = GetPageCountSql();
            return sql1 + ";" + sql2 + ";" + sql3;
        }




        /// <summary>
        /// 生成分页查询语句(不包含记录总数)
        /// </summary>
        /// <returns></returns>
        public string GetPageSql()
        {
            /*
            StringBuilder sql = new StringBuilder();   
            int start_row_num = (PageIndex - 1)*PageSize + 1;
            sql.AppendFormat(" from {0}", TableName);
            if (!string.IsNullOrEmpty(Where))
                sql.AppendFormat(" where {0}", Where);
            if (!string.IsNullOrEmpty(Group))
                sql.AppendFormat(" group by {0}", Group);
            return
                string.Format(
                    "WITH t AS (SELECT ROW_NUMBER() OVER(ORDER BY {0}) as row_number,{1}{2}) Select * from t where row_number BETWEEN {3} and {4}",
                    Sort, SelectColumn, sql, start_row_num, (start_row_num + PageSize - 1));
             */


            return string.Format("SELECT {0} FROM (SELECT TOP {1} *,ROW_NUMBER() OVER(ORDER BY {2}) AS ROWNUM FROM {3} WHERE {4}) AS TMP1 WHERE ROWNUM>{5} ",
                    SelectColumn, PageIndex * PageSize, Sort, TableName, Where, PageSize * (PageIndex - 1));

        }
    }
}
