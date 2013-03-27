﻿using System;
using System.Collections.Generic;
using System.Text;

using MeYoung.Model;
using System.Data.SqlClient;
using System.Data;
using System.Data.Transaction;

namespace MeYoung.DAL
{
    public class CouponDAL : MeYoung.IDAL.ICoupon
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

        
        public CouponDAL()
        {
            this.dbHelper = new SqlHelp("dbType", "connString");
        }
        public CouponDAL(Transaction trans)
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
        public int Add(MeYoung.Model.Coupon model)
        {
            return dbHelper.Add("[Coupon]", "CouponID", model);
        }
        #endregion

        #region Get Model
        /// <summary>
        /// Get Model
        /// </summary>
        /// <param name="CouponID">主键</param>
        /// <returns></returns>
        public MeYoung.Model.Coupon GetModelByPrimaryKey(int CouponID)
        {
            string where = "CouponID = @pk";
            this.dbHelper.Parameters.Add(new SqlParameter("@pk", CouponID));
            return GetModel(where);

        }
        /// <summary>
        /// Get Model
        /// </summary>
        /// <param name="where">不包括 'where'</param>
        /// <returns></returns>
        public MeYoung.Model.Coupon GetModel(string where)
        {
            StringBuilder strSql = new StringBuilder("select top 1 * from [Coupon]");
            if (!string.IsNullOrEmpty(where))
                strSql.Append(" where 1=1  and " + where);
            DataTable table = dbHelper.ExecuteTable(strSql.ToString());
            Coupon model = new Coupon(table);
            return model;
        }
        #endregion

        #region Update
        /// <summary>
        /// Update Method
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update(MeYoung.Model.Coupon model)
        {
            return dbHelper.Update("[Coupon]", "CouponID", model);
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
            string sql = "select * from [Coupon] ";
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
            string sql = "select * from [Coupon] ";
            if (!string.IsNullOrEmpty(where))
                sql += " where 1=1 and " + where;
            return dbHelper.ExecuteDataSet(sql);
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="CouponID">主键</param>
        /// <returns></returns>
        public int Delete(int CouponID)
        {
            string where = "CouponID = @pk";
            this.dbHelper.Parameters.Add(new SqlParameter("@pk", CouponID));
            return Delete(where);

        }
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="where">不包括 'where'</param>
        /// <returns></returns>
        public int Delete(string where)
        {
            StringBuilder strSql = new StringBuilder("Delete from [Coupon]");
            if (!string.IsNullOrEmpty(where))
                strSql.Append(" where 1=1 and " + where);
            int result = dbHelper.ExecuteNonQuery(strSql.ToString());

            return result;
        }

        #endregion

        #endregion
        
        
        
    }
}