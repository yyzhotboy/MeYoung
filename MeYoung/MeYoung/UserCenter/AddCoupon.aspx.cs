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
    public partial class AddCoupon : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                if (Session["U_id"] == null)
                {
                    Response.Redirect("~/User/Login/");
                }
                BindShop();
            }
        }
        private void BindShop()
        {
            MeYoung.BLL.Company bll = new Company();
            DataTable dt = bll.GetList("UserID=" + Convert.ToInt32(Session["U_id"]));
            ddl_Shop.DataSource = dt;
            ddl_Shop.DataTextField = "CompanyName";
            ddl_Shop.DataValueField = "CompanyID";
            ddl_Shop.DataBind();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            MeYoung.BLL.Coupon bll = new Coupon();
            MeYoung.Model.Coupon model = new Model.Coupon();
            model.CouponTitle = tb_Titile.Text;
            model.RelateID = Convert.ToInt32(ddl_Shop.SelectedValue);
            model.RelateType = 0;
            model.CouponDetail = tb_Detail.Text;
            model.CouponStartTime = Convert.ToDateTime(tb_Start.Text);
            model.CouponEndTime = Convert.ToDateTime(tb_End.Text);
            bll.Add(model);
        }
    }
}