using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MeYoung.Model;
namespace MeYoung.IDAL
{
	public interface IShop
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
        int Add(MeYoung.Model.Shop model);

        /// <summary>
        /// Update 
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns></returns>
        int Update(MeYoung.Model.Shop model);

        #region Get Model
        /// <summary>
        /// Get Model
        /// </summary>
        /// <param name="ShopID">主键</param>
        /// <returns></returns>
        MeYoung.Model.Shop GetModelByPrimaryKey(int ShopID);

        /// <summary>
        /// Get Model
        /// </summary>
        /// <param name="where">不包括 'where'</param>
        /// <returns></returns>
        MeYoung.Model.Shop GetModel(string where);

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
        /// <param name="ShopID">主键</param>
        /// <returns></returns>
        int Delete(int ShopID);

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="where">不包括 'where'</param>
        /// <returns></returns>
        int Delete(string where);


        #endregion 
	}
}