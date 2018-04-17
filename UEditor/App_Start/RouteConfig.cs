using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Web.Models;

namespace Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

          
            //编辑器使用 
            routes.MapRoute(
                "Sop_Handler", // Route name
                "SopHandler/{action}/{id}", // URL with parameters
                new { controller = "SopHandler", action = "Handler", id = UrlParameter.Optional }
            ).RouteHandler = new UeRouteHandlerHelper();


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
  
}
