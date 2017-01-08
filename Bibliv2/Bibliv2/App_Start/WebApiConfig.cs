using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Bibliv2
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();
            /*
            config.Routes.MapHttpRoute(
            name: "custom1",
                routeTemplate: "{action}/{id}",
                defaults: new { controller = "Livres", id = RouteParameter.Optional }
            );
            */
            config.Routes.MapHttpRoute(
                name: "cc",
                 routeTemplate: "api/{controller}/{action}/{id}",
                 defaults: new { id = RouteParameter.Optional });
            /*
            config.Routes.MapHttpRoute(
                name:"DefaultApi",
               routeTemplate: "api/{controller}/{id}",
               defaults: new { action = "DefaultAction", id = System.Web.Http.RouteParameter.Optional }
            );
            */
            config.Routes.MapHttpRoute(
               name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
