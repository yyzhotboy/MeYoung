using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MeYoung.BLL;

namespace MeYoung.UserCenter
{
    public partial class AddMail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["U_id"] == null)
            {
                Response.Redirect("~/User/Login/");
            }
        }

        protected void btn_Publish_Click(object sender, EventArgs e)
        {
            string strType = fu_Img.FileName.Substring(fu_Img.FileName.LastIndexOf("."));
            if (strType == ".jpg" || strType == ".png" || strType == ".jpeg")
            {
                try
                {
                    string fileName = DateTime.Now.ToString("yyyyMMddhhmmssfff") +
                                      fu_Img.FileName.Substring(fu_Img.FileName.LastIndexOf("."));
                    string strPath = Server.MapPath("~") + "\\UserImage\\CompanyImg\\" + fileName;
                    fu_Img.SaveAs(strPath);
                    MeYoung.BLL.Mail bll = new Mail();
                    MeYoung.Model.Mail model = new Model.Mail();
                    string strPoint = hd_id.Value;
                    string[] point = strPoint.Split('，');
                    model.MailAddress = tb_Address.Text;
                    model.MailImg = fileName;
                    model.MailJing = Convert.ToDecimal(point[1]);
                    model.MailWei = Convert.ToDecimal(point[0]);
                    model.MailName = tb_name.Text;
                    model.UserID = Convert.ToInt32(Session["U_id"]);
                    int iFlag = bll.Add(model);
                    if (iFlag > 0)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "script2", "alert('添加成功!');", true);
                        return;
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "script2", "alert('添加失败!');", true);
                        return;
                    }

                }
                catch (Exception ex)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "script2", "alert('上传失败!');", true);
                    return;
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "script2", "alert('文件格式不正确!');", true);
                return;
            }
        }
    }
}