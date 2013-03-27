using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace MeYoung.User
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void button2_Click(object sender, EventArgs e)
        {
            string strResult = MyLogin(tb_name.Text, tb_pwd.Text, tb_check.Text);
            if (strResult == "登录成功")
            {
                Response.Redirect("~/UserCenter/Infocenter/");
                //if (Request.QueryString["url"] != null)
                //{
                //    Response.Redirect("../" + Request.QueryString["url"]);
                //}
                //else
                //{
                //    Response.Redirect("../Index.html");
                //}
            }
            else
            {
                Common.MessageBox.AjaxShow(this, strResult);
            }
        }

        /// <summary>
        /// 登录方法
        /// </summary>
        /// <param name="txtName"></param>
        /// <param name="txtPwd"></param>
        /// <param name="imagecode"></param>
        /// <param name="ilock"></param>
        /// <returns></returns>
        public string MyLogin(string txtName, string txtPwd, string imagecode)
        {
            string S_imagecode = string.Empty;
            if (Session["CheckCode"] != null)
            {
                S_imagecode = Session["CheckCode"].ToString();
            }
            if (imagecode != S_imagecode)
            {
                //Common.MessageBox.AjaxShow(this, );
                return "验证码输入错误!";
            }
            MeYoung.BLL.User users = new BLL.User();
            Dictionary<string, object> par = new Dictionary<string, object>();
            par.Add("@UserName", txtName);
            par.Add("@UserPwd", Common.Security.GetMD5(txtPwd));
            users.Parameters = par;
            MeYoung.Model.User model = users.GetModelByWhere("UserName=@UserName and UserPwd=@UserPwd ");
            if (model.UserID > 0)
            {
               
                Session["U_id"] = model.UserID;
                Session["U_name"] = model.UserName;              
                return "登录成功";
            }
            else
            {
                //Common.MessageBox.AjaxShow(this, );
                return "用户名或密码错误，请重新输入!";
            }
        }
    }
}