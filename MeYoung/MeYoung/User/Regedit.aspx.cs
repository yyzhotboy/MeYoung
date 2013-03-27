using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace MeYoung.User
{
    public partial class Regedit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Regedit));
        }

        [AjaxPro.AjaxMethod]
        public int ExitName(string p_strName)
        {
            MeYoung.BLL.User users = new BLL.User();
            MeYoung.Model.User model = new Model.User();         
            Dictionary<string, object> par = new Dictionary<string, object>();
            par.Add("@UserName", p_strName);
            users.Parameters = par;
            model = users.GetModelByWhere("UserName=@UserName ");
            return model.UserID;
        }
        [AjaxPro.AjaxMethod]
        public int ExitEmail(string p_strEmail)
        {
            MeYoung.BLL.User users = new BLL.User();
            MeYoung.Model.User model = new Model.User();   
            Dictionary<string, object> par = new Dictionary<string, object>();
            par.Add("@Email", p_strEmail);
            users.Parameters = par;
            model = users.GetModelByWhere("Email=@Email ");
            return model.UserID;
        }

        protected void button2_Click(object sender, EventArgs e)
        {
            string strResult = RegeditUser(tb_name.Text, tb_pwd.Text, tb_email.Text, tb_phone.Text);
            Common.MessageBox.AjaxShow(this, strResult);
        }
        private string RegeditUser(string name,  string pwd, string mail, string phone)
        {
            MeYoung.Model.User model = new Model.User();
            model.UserName = name;
            model.Email = mail;
          
            model.Phone = phone;
            model.UserPwd = Common.Security.GetMD5(pwd);

            MeYoung.BLL.User users = new BLL.User();
            int i = users.Add(model);
            if (i > 0)
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "script2", "alert('注册成功!');", true);
                return "注册成功";
            }
            else
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "script2", "alert('注册失败!');", true);
                return "注册失败";
            }
        }
    }
}