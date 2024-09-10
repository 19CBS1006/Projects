using System;
using System.Web.Routing;

namespace Cricket_ScoreBoard
{
    /// <summary>
    /// Gloabal ASAX File
    /// </summary>
    public class Global : System.Web.HttpApplication
    {
        /// <summary>
        /// Application Start
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Start(object sender, EventArgs e)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NCaF5cXmZCeUx0THxbf1x0ZFZMY1RbRXNPIiBoS35RckVlW3ZfdXddQ2ReVkR0");
            RouteConfig.RouteConfiguration(RouteTable.Routes);
        }

        protected void Application_End(object sender, EventArgs e)
        {
            Session.Clear(); Session.Abandon();
        }
        /*
         /// <summary>
        /// Defining Objects
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            Uri url = Request.Url;
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }*/
    }
}       