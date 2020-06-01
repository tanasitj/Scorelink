using System.Web.Mvc;
using System.Web.Routing;

namespace Scorelink.web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
<<<<<<< Updated upstream
                defaults: new { controller = "Home", action = "Login", id = UrlParameter.Optional }
=======
                defaults: new { controller = "Digitize", action = "Preview", id = UrlParameter.Optional }
                //defaults: new { controller = "Home", action = "Login", id = UrlParameter.Optional }
>>>>>>> Stashed changes
            );
        }
    }
}
