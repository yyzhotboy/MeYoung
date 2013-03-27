using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MeYoung.UserCenter
{
    public partial class Infocenter : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["U_id"] == null)
            {
                Response.Redirect("~/User/Login/");
            }
        }
    }
}