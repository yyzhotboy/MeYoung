using System; 
using System.Text;
using System.Collections.Generic;
namespace MeYoung.Model
{
 	/// <summary>
 	///Mail
 	/// </summary>
  	public class Mail
	{
		#region	字段
				private string _Transaction_TableName = "Mail";
		        private int  _Transaction_type = 1;
                private string _Transaction_PrimaryKey = "MailID";        
		        private int _mailid;
				private string _mailname = null ;
				private string _mailimg = null ;
				private string _mailaddress = null ;
				private decimal? _mailjing = null ;
				private decimal? _mailwei = null ;
				private int? _userid = null ;
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
		/// MailID
        /// </summary>		
        public int MailID
        {
            get{ return _mailid; }
            set{ _mailid = value; }
        }        
			
		/// <summary>
		/// MailName
        /// </summary>		
        public string MailName
        {
            get{ return _mailname; }
            set{ _mailname = value; }
        } 			
			
		/// <summary>
		/// MailImg
        /// </summary>		
        public string MailImg
        {
            get{ return _mailimg; }
            set{ _mailimg = value; }
        } 			
			
		/// <summary>
		/// MailAddress
        /// </summary>		
        public string MailAddress
        {
            get{ return _mailaddress; }
            set{ _mailaddress = value; }
        } 			
			
		/// <summary>
		/// MailJing
        /// </summary>		
        public decimal? MailJing
        {
            get{ return _mailjing; }
            set{ _mailjing = value; }
        } 			
			
		/// <summary>
		/// MailWei
        /// </summary>		
        public decimal? MailWei
        {
            get{ return _mailwei; }
            set{ _mailwei = value; }
        } 			
			
		/// <summary>
		/// UserID
        /// </summary>		
        public int? UserID
        {
            get{ return _userid; }
            set{ _userid = value; }
        } 			
			
				#endregion
		
		#region 构造方法
        public Mail()
        { }

        public Mail(System.Data.DataTable table)
        {
            this.SetModel(table);
        }
        #endregion
        
        #region 私有方法
        private void SetModel(System.Data.DataTable table)
        {
        	if (table != null && table.Rows.Count > 0)
			{
				if (table.Rows[0]["MailID"] != null && !string.IsNullOrEmpty(table.Rows[0]["MailID"].ToString()))
        			this.MailID = Convert.ToInt32(table.Rows[0]["MailID"]);
        		 if( table.Rows[0]["MailName"] != null)
	        		this.MailName = Convert.ToString(table.Rows[0]["MailName"]);
	        	 if( table.Rows[0]["MailImg"] != null)
	        		this.MailImg = Convert.ToString(table.Rows[0]["MailImg"]);
	        	 if( table.Rows[0]["MailAddress"] != null)
	        		this.MailAddress = Convert.ToString(table.Rows[0]["MailAddress"]);
	        	if (table.Rows[0]["MailJing"] != null && !string.IsNullOrEmpty(table.Rows[0]["MailJing"].ToString()))
        			this.MailJing = Convert.ToDecimal(table.Rows[0]["MailJing"]);
        		if (table.Rows[0]["MailWei"] != null && !string.IsNullOrEmpty(table.Rows[0]["MailWei"].ToString()))
        			this.MailWei = Convert.ToDecimal(table.Rows[0]["MailWei"]);
        		if (table.Rows[0]["UserID"] != null && !string.IsNullOrEmpty(table.Rows[0]["UserID"].ToString()))
        			this.UserID = Convert.ToInt32(table.Rows[0]["UserID"]);
        		        		
        	}
        	
        }
        #endregion

        		
   
	}
}


