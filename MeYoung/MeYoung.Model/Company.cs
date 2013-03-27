using System; 
using System.Text;
using System.Collections.Generic;
namespace MeYoung.Model
{
 	/// <summary>
 	///Company
 	/// </summary>
    public class Company
    {
        #region	字段
        private string _Transaction_TableName = "Company";
        private int _Transaction_type = 1;
        private string _Transaction_PrimaryKey = "CompanyID";
        private int _companyid;
        private string _companyname = null;
        private string _companyimg = null;
        private decimal? _companyjing = null;
        private decimal? _companywei = null;
        private string _companyaddress = null;
        private int? _userid = null;
        private int? _typeid = null;
 	    private int? _mailid = null;
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
        /// 商家id
        /// </summary>		
        public int CompanyID
        {
            get { return _companyid; }
            set { _companyid = value; }
        }

        /// <summary>
        /// CompanyName
        /// </summary>		
        public string CompanyName
        {
            get { return _companyname; }
            set { _companyname = value; }
        }

        /// <summary>
        /// CompanyImg
        /// </summary>		
        public string CompanyImg
        {
            get { return _companyimg; }
            set { _companyimg = value; }
        }

        /// <summary>
        /// CompanyJing
        /// </summary>		
        public decimal? CompanyJing
        {
            get { return _companyjing; }
            set { _companyjing = value; }
        }

        /// <summary>
        /// CompanyWei
        /// </summary>		
        public decimal? CompanyWei
        {
            get { return _companywei; }
            set { _companywei = value; }
        }

        /// <summary>
        /// CompanyAddress
        /// </summary>		
        public string CompanyAddress
        {
            get { return _companyaddress; }
            set { _companyaddress = value; }
        }

        /// <summary>
        /// UserID
        /// </summary>		
        public int? UserID
        {
            get { return _userid; }
            set { _userid = value; }
        }

        /// <summary>
        /// TypeID
        /// </summary>		
        public int? TypeID
        {
            get { return _typeid; }
            set { _typeid = value; }
        }

 	    public int? MailID
 	    {
            get { return _mailid; }
            set { _mailid = value; }
 	    }
        #endregion

        #region 构造方法
        public Company()
        { }

        public Company(System.Data.DataTable table)
        {
            this.SetModel(table);
        }
        #endregion

        #region 私有方法
        private void SetModel(System.Data.DataTable table)
        {
            if (table != null && table.Rows.Count > 0)
            {
                if (table.Rows[0]["CompanyID"] != null && !string.IsNullOrEmpty(table.Rows[0]["CompanyID"].ToString()))
                    this.CompanyID = Convert.ToInt32(table.Rows[0]["CompanyID"]);
                if (table.Rows[0]["CompanyName"] != null)
                    this.CompanyName = Convert.ToString(table.Rows[0]["CompanyName"]);
                if (table.Rows[0]["CompanyImg"] != null)
                    this.CompanyImg = Convert.ToString(table.Rows[0]["CompanyImg"]);
                if (table.Rows[0]["CompanyJing"] != null && !string.IsNullOrEmpty(table.Rows[0]["CompanyJing"].ToString()))
                    this.CompanyJing = Convert.ToDecimal(table.Rows[0]["CompanyJing"]);
                if (table.Rows[0]["CompanyWei"] != null && !string.IsNullOrEmpty(table.Rows[0]["CompanyWei"].ToString()))
                    this.CompanyWei = Convert.ToDecimal(table.Rows[0]["CompanyWei"]);
                if (table.Rows[0]["CompanyAddress"] != null)
                    this.CompanyAddress = Convert.ToString(table.Rows[0]["CompanyAddress"]);
                if (table.Rows[0]["UserID"] != null && !string.IsNullOrEmpty(table.Rows[0]["UserID"].ToString()))
                    this.UserID = Convert.ToInt32(table.Rows[0]["UserID"]);
                if (table.Rows[0]["TypeID"] != null && !string.IsNullOrEmpty(table.Rows[0]["TypeID"].ToString()))
                    this.TypeID = Convert.ToInt32(table.Rows[0]["TypeID"]);
                if (table.Rows[0]["MailID"] != null && !string.IsNullOrEmpty(table.Rows[0]["MailID"].ToString()))
                    this.MailID = Convert.ToInt32(table.Rows[0]["MailID"]);

            }

        }
        #endregion



    }
}


