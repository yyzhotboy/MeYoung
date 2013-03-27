using System;
using System.Collections.Generic;
using System.Text;

using MeYoung.Model;
using System.Data.SqlClient;
using System.Data;
using System.Data.Transaction;

namespace MeYoung.DAL
{
    public class ShopDAL : MeYoung.IDAL.IShop
    {
    	#region Must have
        private SqlHelp dbHelper;
        public Dictionary<string, object> Parameters
        {
            set
            {
                int i = 0;
                this.dbHelper.Parameters.Clear();
                foreach (KeyValuePair<string, object> item in value)
                {
                    SqlParameter sparam = new SqlParameter(item.Key, item.Value);
                    this.dbHelper.Parameters.Add(sparam);
                }
            }
            
        }

        
        public ShopDAL()
        {
            this.dbHelper = new SqlHelp("dbType", "connString");
        }
        public ShopDAL(Transaction trans)
        {
            this.dbHelper = trans.GetSqlHelp();
        }
        #endregion
        
        #region 通用方法


        #region Add
        /// <summary>
        /// Add Method
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns></returns>
        public int Add(MeYoung.Model.Shop model)
        {
            return dbHelper.Add("[Shop]", "ShopID", model);
        }
        #endregion

        #region Get Model
        /// <summary>
        /// Get Model
        /// </summary>
        /// <param name="ShopID">主键</param>
        /// <returns></returns>
        public MeYoung.Model.Shop GetModelByPrimaryKey(int ShopID)
        {
            string where = "ShopID = @pk";
            this.dbHelper.Parameters.Add(new SqlParameter("@pk", ShopID));
            return GetModel(where);

        }
        /// <summary>
        /// Get Model
        /// </summary>
        /// <param name="where">不包括 'where'</param>
        /// <returns></returns>
        public MeYoung.Model.Shop GetModel(string where)
        {
            StringBuilder strSql = new StringBuilder("select top 1 * from [Shop]");
            if (!string.IsNullOrEmpty(where))
                strSql.Append(" where 1=1  and " + where);
            DataTable table = dbHelper.ExecuteTable(strSql.ToString());
            Shop model = new Shop(table);
            return model;
        }
        #endregion

        #region Update
        /// <summary>
        /// Update Method
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update(MeYoung.Model.Shop model)
        {
            return dbHelper.Update("[Shop]", "ShopID", model);
        }
        #endregion

        #region Get List
        /// <summary>
        /// Get List
        /// </summary>
        /// <param name="where">不包括 'where'</param>
        /// <returns></returns>
        public DataTable GetList(string where)
        {
            string sql = "select * from [Shop] ";
            if (!string.IsNullOrEmpty(where))
                sql += " where 1=1 and " + where;
            return dbHelper.ExecuteTable(sql);
        }
        /// <summary>
        /// Get List
        /// </summary>
        /// <param name="where">不包括 'where'</param>
        /// <returns></returns>
        public DataSet GetList1(string where)
        {
            string sql = "select * from [Shop] ";
            if (!string.IsNullOrEmpty(where))
                sql += " where 1=1 and " + where;
            return dbHelper.ExecuteDataSet(sql);
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="ShopID">主键</param>
        /// <returns></returns>
        public int Delete(int ShopID)
        {
            string where = "ShopID = @pk";
            this.dbHelper.Parameters.Add(new SqlParameter("@pk", ShopID));
            return Delete(where);

        }
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="where">不包括 'where'</param>
        /// <returns></returns>
        public int Delete(string where)
        {
            StringBuilder strSql = new StringBuilder("Delete from [Shop]");
            if (!string.IsNullOrEmpty(where))
                strSql.Append(" where 1=1 and " + where);
            int result = dbHelper.ExecuteNonQuery(strSql.ToString());

            return result;
        }

        #endregion

        #endregion
        
        
        
    }
}