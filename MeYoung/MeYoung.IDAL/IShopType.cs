using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MeYoung.Model;
namespace MeYoung.IDAL
{
	public interface IShopType
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

        /// <summary>
        /// Add 
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns></returns>
        int Add(MeYoung.Model.ShopType model);

        /// <summary>
        /// Update 
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns></returns>
        int Update(MeYoung.Model.ShopType model);

        #region Get Model
        /// <summary>
        /// Get Model
        /// </summary>
        /// <param name="ShopTypeID">主键</param>
        /// <returns></returns>
        MeYoung.Model.ShopType GetModelByPrimaryKey(int ShopTypeID);

        /// <summary>
        /// Get Model
        /// </summary>
        /// <param name="where">不包括 'where'</param>
        /// <returns></returns>
        MeYoung.Model.ShopType GetModel(string where);

        #endregion

        /// <summary>
        /// Get List
        /// </summary>
        /// <param name="where">不包括 'where'</param>
        /// <returns></returns>
        DataTable GetList(string where);
        
        /// <summary>
        /// Get List
        /// </summary>
        /// <param name="where">不包括 'where'</param>
        /// <returns></returns>
        DataSet GetList1(string where);

        /// <summary>
        /// Delete 
        /// </summary>
        /// <param name="ShopTypeID">主键</param>
        /// <returns></returns>
        int Delete(int ShopTypeID);

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="where">不包括 'where'</param>
        /// <returns></returns>
        int Delete(string where);


        #endregion 
	}
}