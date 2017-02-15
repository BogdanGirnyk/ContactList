using System.Web.Mvc;
using System.Web.Routing;

namespace ContactList.App_Start
{
    public class RouteConfig
    {
        public static void Configure(RouteCollection routes)
        {

            routes.MapRoute(
                name: "404-NotFound",
                url: "NotFound",
                defaults: new { controller = "Error", action = "NotFound" }
            );

            routes.MapRoute(
                name: "500-Error",
                url: "Error",
                defaults: new { controller = "Error", action = "Error" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}/{PhId}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional, PhId = UrlParameter.Optional }
            );



            routes.MapRoute(
                name: "NotFound",
                url: "{*url}",
                defaults: new { controller = "Error", action = "NotFound" }
            );


        }
    }
}