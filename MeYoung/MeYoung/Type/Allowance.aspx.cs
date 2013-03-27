using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MeYoung.BLL;

namespace MeYoung.Type
{
    public partial class Allowance : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                BindCoupon();
            }
        }

        private void BindCoupon()
        {
            MeYoung.BLL.DBCommonBLL bll = new DBCommonBLL();
            string type = Page.RouteData.Values["Type"] as string;
            DataSet ds = bll.GetPageList("View_DaZhe", "TypeID=" + type, "CouponID ASC",Pager.PageSize, Pager.CurrentPageIndex, true, true);
            R_Type.DataSource = ds;
            R_Type.DataBind();
            try
            {
                Pager.RecordCount = int.Parse(ds.Tables[1].Rows[0][0].ToString());
            }
            catch
            {
                Pager.RecordCount = 0;
            }
        }

        protected void Pager_PageChanged(object sender, EventArgs e)
        {
            this.BindCoupon();
            if (Pager.CurrentPageIndex > 1)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "animate", "<script type='text/javascript'>$('html, body').animate({ scrollTop: 150 }, 1000);</script>");
            }
        }
    }
}