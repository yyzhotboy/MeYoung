using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace MeYoung
{
    public class Global : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {


            routes.MapPageRoute(
                "Default", // Route name
                "", // URL with parameters
                "~/Index.aspx" // Parameter defaults
            );
           routes.MapPageRoute(
                "WebForm1",
                 "{folder}/{webform}",
                "~/{folder}/{webform}.aspx"
            );
           routes.MapPageRoute(
                "WebForm2",
                "{floder}/{webform}/{parameter}",
                "~/{floder}/{webform}.aspx"
          );

        }
        protected void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes(RouteTable.Routes);

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}