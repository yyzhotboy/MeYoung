using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MeYoung.Map
{
    public partial class MapPoint : System.Web.UI.Page
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }
        //[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        //public string Point(string p_strPoint)
        //{
        //    Session["Point"] = p_strPoint;

        //    return p_strPoint;
        //}
       //[AjaxPro.AjaxMethod]
       // public string GetPoint(int id)
       //  {
       //      RoadAssistant.Model.CompanyBaseInfo ctx = new RoadAssistant.BLL.CompanyBaseInfo().GetModel(id);
       //      if(ctx.CompanyID >0)
       //      {
       //          return ctx.CompanyLongitude.ToString() + "," + ctx.CompanyLatitude;
       //      }
       //      else
       //      {
       //          return "";
       //      }
       //  }
        //[WebMethod]
        //public static string Point(string p_strPoint)
        //{
           
        //}
    }
}