using System; 
using System.Text;
using System.Collections.Generic; 
namespace MeYoung.Model
{
 	/// <summary>
 	///ShopType
 	/// </summary>
  	public class ShopType
	{
		#region	字段
				private string _Transaction_TableName = "ShopType";
		        private int  _Transaction_type = 1;
                private string _Transaction_PrimaryKey = "ShopTypeID";        
		        private int _shoptypeid;
				private string _shoptypename = null ;
						#endregion
		
		#region 属性
		
		/// <summary>
        /// 表名 不可修改 事物专用
        /// </summary>		
        public string Transaction_TableName
        {
            get { return _Transaction_TableName; }
        }
        /// <summary>
        /// 主键 不可修改 事物专用
        /// </summary>		
        public string Transaction_PrimaryKey
        {
            get { return _Transaction_PrimaryKey; }
        }
        /// <summary>
        /// 类型 事物专用 1添加  2修改
        /// </summary>		
        public int Transaction_type
        {
            get { return _Transaction_type; }
            set { _Transaction_type = value; }
        }
        
		/// <summary>
		/// ShopTypeID
        /// </summary>		
        public int ShopTypeID
        {
            get{ return _shoptypeid; }
            set{ _shoptypeid = value; }
        }        
			
		/// <summary>
		/// ShopTypeName
        /// </summary>		
        public string ShopTypeName
        {
            get{ return _shoptypename; }
            set{ _shoptypename = value; }
        } 			
			
				#endregion
		
		#region 构造方法
        public ShopType()
        { }

        public ShopType(System.Data.DataTable table)
        {
            this.SetModel(table);
        }
        #endregion
        
        #region 私有方法
        private void SetModel(System.Data.DataTable table)
        {
        	if (table != null && table.Rows.Count > 0)
			{
				if (table.Rows[0]["ShopTypeID"] != null && !string.IsNullOrEmpty(table.Rows[0]["ShopTypeID"].ToString()))
        			this.ShopTypeID = Convert.ToInt32(table.Rows[0]["ShopTypeID"]);
        		 if( table.Rows[0]["ShopTypeName"] != null)
	        		this.ShopTypeName = Convert.ToString(table.Rows[0]["ShopTypeName"]);
	        	        		
        	}
        	
        }
        #endregion

        		
   
	}
}


