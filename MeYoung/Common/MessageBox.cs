using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class MessageBox
    {
        private MessageBox()
        {
        }

        /// <summary>
        /// 显示消息提示对话框
        /// </summary>
        /// <param name="page">当前页面指针，一般为this</param>
        /// <param name="msg">提示信息</param>
        public static void Show(System.Web.UI.Page page, string msg)
        {
            msg = msg.Replace("\r\n", "\\r\\n");//杨栋添加
            page.ClientScript.RegisterStartupScript(page.GetType(), "message", "<script language='javascript' defer>alert('" + msg + "');</script>");
        }
        /// <summary>
        /// （如果页面引用了ajax控件，需要使用此方法）显示消息提示对话框
        /// </summary>
        /// <param name="page">当前页面指针，一般为this</param>
        /// <param name="msg">提示信息</param>
        public static void AjaxShow(System.Web.UI.Page page, string msg)
        {
            msg = msg.Replace("\r\n", "\\r\\n");//杨栋添加
            System.Web.UI.ScriptManager.RegisterStartupScript(page, page.GetType(), "message", "alert('" + msg + "');", true);
        }
        /// <summary>
        /// （如果页面引用了ajax控件，需要使用此方法）且使用了A标签
        /// </summary>
        /// <param name="page">当前页面指针，一般为this</param>
        /// <param name="msg">提示信息</param>
        /// <param name="a">A标签</param>
        public static void AjaxShow(System.Web.UI.Page page, string msg, string a)
        {
            msg = msg.Replace("\r\n", "\\r\\n");//杨栋添加
            System.Web.UI.ScriptManager.RegisterStartupScript(page, page.GetType(), "message", "alert('" + msg + "');$(\".easyui-linkbutton\").linkbutton();", true);
        }
        /// <summary>
        /// 控件点击 消息确认提示框
        /// </summary>
        /// <param name="page">当前页面指针，一般为this</param>
        /// <param name="msg">提示信息</param>
        public static void ShowConfirm(System.Web.UI.WebControls.WebControl Control, string msg)
        {
            //Control.Attributes.Add("onClick","if (!window.confirm('"+msg+"')){return false;}");
            Control.Attributes.Add("onclick", "return confirm('" + msg + "');");
        }

        /// <summary>
        /// 显示消息提示对话框，并进行页面跳转
        /// </summary>
        /// <param name="page">当前页面指针，一般为this</param>
        /// <param name="msg">提示信息</param>
        /// <param name="url">跳转的目标URL</param>
        public static void ShowAndRedirect(System.Web.UI.Page page, string msg, string url)
        {
            StringBuilder Builder = new StringBuilder();
            Builder.Append("<script language='javascript' defer>");
            Builder.AppendFormat("alert('{0}');", msg);
            Builder.AppendFormat("top.location.href='{0}'", url);
            Builder.Append("</script>");
            page.ClientScript.RegisterStartupScript(page.GetType(), "message", Builder.ToString());

        }
        /// <summary>
        /// 输出自定义脚本信息
        /// </summary>
        /// <param name="page">当前页面指针，一般为this</param>
        /// <param name="script">输出脚本</param>
        public static void ResponseScript(System.Web.UI.Page page, string script)
        {
            page.ClientScript.RegisterStartupScript(page.GetType(), "message", "<script language='javascript' defer>" + script + "</script>");

        }

    }
}
