using System; 
using System.Text;
using System.Collections.Generic;
namespace MeYoung.Model
{
 	/// <summary>
 	///User
 	/// </summary>
    public class User
    {
        #region	字段
        private string _Transaction_TableName = "User";
        private int _Transaction_type = 1;
        private string _Transaction_PrimaryKey = "UserID";
        private int _userid;
        private string _username = null;
        private string _userpwd = null;
        private string _email = null;
        private string _phone = null;
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
        /// UserID
        /// </summary>		
        public int UserID
        {
            get { return _userid; }
            set { _userid = value; }
        }

        /// <summary>
        /// UserName
        /// </summary>		
        public string UserName
        {
            get { return _username; }
            set { _username = value; }
        }

        /// <summary>
        /// UserPwd
        /// </summary>		
        public string UserPwd
        {
            get { return _userpwd; }
            set { _userpwd = value; }
        }

        /// <summary>
        /// Email
        /// </summary>		
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        /// <summary>
        /// Phone
        /// </summary>		
        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }

        #endregion

        #region 构造方法
        public User()
        { }

        public User(System.Data.DataTable table)
        {
            this.SetModel(table);
        }
        #endregion

        #region 私有方法
        private void SetModel(System.Data.DataTable table)
        {
            if (table != null && table.Rows.Count > 0)
            {
                if (table.Rows[0]["UserID"] != null && !string.IsNullOrEmpty(table.Rows[0]["UserID"].ToString()))
                    this.UserID = Convert.ToInt32(table.Rows[0]["UserID"]);
                if (table.Rows[0]["UserName"] != null)
                    this.UserName = Convert.ToString(table.Rows[0]["UserName"]);
                if (table.Rows[0]["UserPwd"] != null)
                    this.UserPwd = Convert.ToString(table.Rows[0]["UserPwd"]);
                if (table.Rows[0]["Email"] != null)
                    this.Email = Convert.ToString(table.Rows[0]["Email"]);
                if (table.Rows[0]["Phone"] != null)
                    this.Phone = Convert.ToString(table.Rows[0]["Phone"]);

            }

        }
        #endregion



    }
}


