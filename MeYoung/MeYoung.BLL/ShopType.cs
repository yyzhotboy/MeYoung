﻿using System;
using System.Collections.Generic;
using System.Text;

using System.Collections;
using System.Data;
using System.Data.Common;
//using System.Data.Transaction;
using MeYoung.Model;
using MeYoung.IDAL;
using MeYoung.DAL;

namespace MeYoung.BLL
{
    public class ShopType
    {
    	#region Must have

        private MeYoung.IDAL.IShopType dal = new MeYoung.DAL.ShopTypeDAL();
        public ShopType() { }
        public ShopType(Dictionary<string, object> parameters)
        {
            dal.Parameters = parameters;
        }
        public Dictionary<string, object> Parameters
        {
			set{ dal.Parameters = value; }
        }
        #endregion
        
        
        #region 通用方法


        #region Add
        /// <summary>
        /// Add Method
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns></returns>
        public int Add(MeYoung.Model.ShopType model)
        {
            return dal.Add(model);
        }
        #endregion

        #region Get Model
        /// <summary>
        /// Get Model
        /// </summary>
        /// <param name="ShopTypeID">主键</param>
        /// <returns></returns>
        public MeYoung.Model.ShopType GetModel(int ShopTypeID)
        {
            return dal.GetModelByPrimaryKey(ShopTypeID);

        }
        /// <summary>
        /// Get Model
        /// </summary>
        /// <param name="where">不包括 'where'</param>
        /// <returns></returns>
        public MeYoung.Model.ShopType GetModelByWhere(string where)
        {
           return dal.GetModel(where);
        }
        #endregion

        #region Update
        /// <summary>
        /// Update Method
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update(MeYoung.Model.ShopType model)
        {
            return dal.Update(model);
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
            return dal.GetList(where);
         }
        
        /// <summary>
        /// Get List
        /// </summary>
        /// <param name="where">不包括 'where'</param>
        /// <returns></returns>
        public DataSet GetList1(string where)
        {
            return dal.GetList1(where);
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="ShopTypeID">主键</param>
        /// <returns></returns>
        public int Delete(int ShopTypeID)
        {
            return dal.Delete(ShopTypeID);

        }
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="where">不包括 'where'</param>
        /// <returns></returns>
        public int Delete(string where)
        {
            return dal.Delete(where);
        }
        /// <summary>
        /// DeleteList
        /// </summary>
        /// <param name="where">idlist id1,id2,id3</param>
        /// <returns></returns>
        public int DeleteList(string idlist)
        {
        	string where = " ShopTypeID in ( " + idlist + " ) ";
            return dal.Delete(where);
        }

        #endregion

        #endregion
        
        
        
    }
}