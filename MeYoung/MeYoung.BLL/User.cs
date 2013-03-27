using System;
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
    public class User
    {
    	#region Must have
        private MeYoung.IDAL.IUser dal = new MeYoung.DAL.UserDAL();//数据库操作类
        public User() { }
        public User(Dictionary<string, object> parameters)
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
        public int Add(MeYoung.Model.User model)
        {
            return dal.Add(model);
        }
        #endregion

        #region Get Model
        /// <summary>
        /// Get Model
        /// </summary>
        /// <param name="UserID">主键</param>
        /// <returns></returns>
        public MeYoung.Model.User GetModel(int UserID)
        {
            return dal.GetModelByPrimaryKey(UserID);

        }
        /// <summary>
        /// Get Model
        /// </summary>
        /// <param name="where">不包括 'where'</param>
        /// <returns></returns>
        public MeYoung.Model.User GetModelByWhere(string where)
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
        public int Update(MeYoung.Model.User model)
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
        /// <param name="UserID">主键</param>
        /// <returns></returns>
        public int Delete(int UserID)
        {
            return dal.Delete(UserID);

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
        	string where = " UserID in ( " + idlist + " ) ";
            return dal.Delete(where);
        }

        #endregion

        #endregion
        
        
        
    }
}