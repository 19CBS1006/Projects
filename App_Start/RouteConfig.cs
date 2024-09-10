using System.Web.Mvc;
using System.Web.Routing;

namespace Cricket_ScoreBoard
{
    /// <summary>
    /// RouteConfig File
    /// </summary>
    public static class RouteConfig
    {
        /// <summary>
        /// Route Configuration
        /// </summary>
        /// <param name="route"></param>
        public static void RouteConfiguration(RouteCollection route)
        {
            route.MapRoute
                (
                    name: "Default",
                    url: "{controller}/{action}/{ID}",
                    defaults: new {controller = "Cricket", action = "UserPage", ID = UrlParameter.Optional }
                );
        }
    }
}