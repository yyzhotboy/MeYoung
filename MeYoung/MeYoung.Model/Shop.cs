using System; 
using System.Text;
using System.Collections.Generic;
namespace MeYoung.Model
{
 	/// <summary>
 	///Shop
 	/// </summary>
    public class Shop
    {
        #region	字段
        private string _Transaction_TableName = "Shop";
        private int _Transaction_type = 1;
        private string _Transaction_PrimaryKey = "ShopID";
        private int _shopid;
        private string _shopname = null;
        private string _shopimg = null;
        private int? _mailid = null;
        private string _shopaddress = null;
        private int? _userid = null;
        private int? _typeid = null;
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
        /// ShopID
        /// </summary>		
        public int ShopID
        {
            get { return _shopid; }
            set { _shopid = value; }
        }

        /// <summary>
        /// ShopName
        /// </summary>		
        public string ShopName
        {
            get { return _shopname; }
            set { _shopname = value; }
        }

        /// <summary>
        /// ShopImg
        /// </summary>		
        public string ShopImg
        {
            get { return _shopimg; }
            set { _shopimg = value; }
        }

        /// <summary>
        /// MailID
        /// </summary>		
        public int? MailID
        {
            get { return _mailid; }
            set { _mailid = value; }
        }

        /// <summary>
        /// ShopAddress
        /// </summary>		
        public string ShopAddress
        {
            get { return _shopaddress; }
            set { _shopaddress = value; }
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

        #endregion

        #region 构造方法
        public Shop()
        { }

        public Shop(System.Data.DataTable table)
        {
            this.SetModel(table);
        }
        #endregion

        #region 私有方法
        private void SetModel(System.Data.DataTable table)
        {
            if (table != null && table.Rows.Count > 0)
            {
                if (table.Rows[0]["ShopID"] != null && !string.IsNullOrEmpty(table.Rows[0]["ShopID"].ToString()))
                    this.ShopID = Convert.ToInt32(table.Rows[0]["ShopID"]);
                if (table.Rows[0]["ShopName"] != null)
                    this.ShopName = Convert.ToString(table.Rows[0]["ShopName"]);
                if (table.Rows[0]["ShopImg"] != null)
                    this.ShopImg = Convert.ToString(table.Rows[0]["ShopImg"]);
                if (table.Rows[0]["MailID"] != null && !string.IsNullOrEmpty(table.Rows[0]["MailID"].ToString()))
                    this.MailID = Convert.ToInt32(table.Rows[0]["MailID"]);
                if (table.Rows[0]["ShopAddress"] != null)
                    this.ShopAddress = Convert.ToString(table.Rows[0]["ShopAddress"]);
                if (table.Rows[0]["UserID"] != null && !string.IsNullOrEmpty(table.Rows[0]["UserID"].ToString()))
                    this.UserID = Convert.ToInt32(table.Rows[0]["UserID"]);
                if (table.Rows[0]["TypeID"] != null && !string.IsNullOrEmpty(table.Rows[0]["TypeID"].ToString()))
                    this.TypeID = Convert.ToInt32(table.Rows[0]["TypeID"]);

            }

        }
        #endregion



    }
}


