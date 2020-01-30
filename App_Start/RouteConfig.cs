using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ProjektTitsOI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //routes.MapMvcAttributeRoutes(); //Enables Attribute Routing


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }

            );

            routes.MapRoute(
                name: "Default2",
                url: "{controller}/{action}/{id}/{rok}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional, rok = UrlParameter.Optional });

        }
    }
}
