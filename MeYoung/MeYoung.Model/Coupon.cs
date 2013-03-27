using System; 
using System.Text;
using System.Collections.Generic;
namespace MeYoung.Model
{
 	/// <summary>
 	///Coupon
 	/// </summary>
  	public class Coupon
	{
		#region	字段
				private string _Transaction_TableName = "Coupon";
		        private int  _Transaction_type = 1;
                private string _Transaction_PrimaryKey = "CouponID";        
		        private int _couponid;
				private string _couponimg = null ;
				private string _coupondetail = null ;
				private DateTime? _couponstarttime = null ;
				private DateTime? _couponendtime = null ;
				private int? _relateid = null ;
				private int? _relatetype = null ;
 	    private string _coupontitle = null; 
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
		/// CouponID
        /// </summary>		
        public int CouponID
        {
            get{ return _couponid; }
            set{ _couponid = value; }
        }        
			
		/// <summary>
		/// CouponImg
        /// </summary>		
        public string CouponImg
        {
            get{ return _couponimg; }
            set{ _couponimg = value; }
        } 			
			
		/// <summary>
		/// CouponDetail
        /// </summary>		
        public string CouponDetail
        {
            get{ return _coupondetail; }
            set{ _coupondetail = value; }
        } 			
			
		/// <summary>
		/// CouponStartTime
        /// </summary>		
        public DateTime? CouponStartTime
        {
            get{ return _couponstarttime; }
            set{ _couponstarttime = value; }
        } 			
			
		/// <summary>
		/// CouponEndTime
        /// </summary>		
        public DateTime? CouponEndTime
        {
            get{ return _couponendtime; }
            set{ _couponendtime = value; }
        } 			
			
		/// <summary>
		/// RelateID
        /// </summary>		
        public int? RelateID
        {
            get{ return _relateid; }
            set{ _relateid = value; }
        } 			
			
		/// <summary>
		/// RelateType
        /// </summary>		
        public int? RelateType
        {
            get{ return _relatetype; }
            set{ _relatetype = value; }
        } 	
		
 	    public string CouponTitle
 	    {
            get { return _coupontitle; }
            set { _coupontitle = value; }
 	    }
			
				#endregion
		
		#region 构造方法
        public Coupon()
        { }

        public Coupon(System.Data.DataTable table)
        {
            this.SetModel(table);
        }
        #endregion
        
        #region 私有方法
        private void SetModel(System.Data.DataTable table)
        {
        	if (table != null && table.Rows.Count > 0)
			{
				if (table.Rows[0]["CouponID"] != null && !string.IsNullOrEmpty(table.Rows[0]["CouponID"].ToString()))
        			this.CouponID = Convert.ToInt32(table.Rows[0]["CouponID"]);
        		 if( table.Rows[0]["CouponImg"] != null)
	        		this.CouponImg = Convert.ToString(table.Rows[0]["CouponImg"]);
	        	 if( table.Rows[0]["CouponDetail"] != null)
	        		this.CouponDetail = Convert.ToString(table.Rows[0]["CouponDetail"]);
	        	if (table.Rows[0]["CouponStartTime"] != null && !string.IsNullOrEmpty(table.Rows[0]["CouponStartTime"].ToString()))
        			this.CouponStartTime = Convert.ToDateTime(table.Rows[0]["CouponStartTime"]);
        		if (table.Rows[0]["CouponEndTime"] != null && !string.IsNullOrEmpty(table.Rows[0]["CouponEndTime"].ToString()))
        			this.CouponEndTime = Convert.ToDateTime(table.Rows[0]["CouponEndTime"]);
        		if (table.Rows[0]["RelateID"] != null && !string.IsNullOrEmpty(table.Rows[0]["RelateID"].ToString()))
        			this.RelateID = Convert.ToInt32(table.Rows[0]["RelateID"]);
        		if (table.Rows[0]["RelateType"] != null && !string.IsNullOrEmpty(table.Rows[0]["RelateType"].ToString()))
        			this.RelateType = Convert.ToInt32(table.Rows[0]["RelateType"]);
                if (table.Rows[0]["CouponTitle"] != null && !string.IsNullOrEmpty(table.Rows[0]["CouponTitle"].ToString()))
                    this.CouponTitle = table.Rows[0]["CouponTitle"].ToString();
        		        		
        	}
        	
        }
        #endregion

        		
   
	}
}


