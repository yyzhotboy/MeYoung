using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MeYoung.BLL;

namespace MeYoung.UserCenter
{
    public partial class AddCompany : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                if (Session["U_id"] == null)
                {
                    Response.Redirect("~/User/Login/");
                }
                BindType();
                BindMail();
            }
        }

        private void BindType()
        {
            MeYoung.BLL.ShopType bll = new ShopType();
            DataTable dt = bll.GetList("");
            ddl_Type.DataSource = dt;
            ddl_Type.DataTextField = "ShopTypeName";
            ddl_Type.DataValueField = "ShopTypeID";
            ddl_Type.DataBind();

        }
        private void BindMail()
        {
            MeYoung.BLL.Mail bll = new Mail();
            DataTable dt = bll.GetList("");
            ddl_Mail.DataSource = dt;
            ddl_Mail.DataTextField = "MailName";
            ddl_Mail.DataValueField = "MailID";
            ddl_Mail.DataBind();

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
                    MeYoung.BLL.Shop bll = new Shop();
                    MeYoung.Model.Shop model = new Model.Shop();
                    string strPoint = hd_id.Value;
                    string[] point = strPoint.Split('，');
                    model.ShopAddress = tb_Address.Text;
                    model.ShopImg = fileName;
                    model.ShopName = tb_name.Text;
                    model.TypeID = Convert.ToInt32(ddl_Type.SelectedValue);
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