using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Routing;
using Util.Extend;

namespace MyUniversity
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            RouteTable.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            ).RouteHandler = new SessionRouteHandler();  //支持Session的Web Api

            RouteTable.Routes.MapHttpRoute(
                name: "RegisterApi",
                routeTemplate: "api/ApiAccount/PostRegister",
                defaults: new { controllers = "ApiAccount" }
            ).RouteHandler = new SessionRouteHandler();  //支持Session的Web Api
        }
    }
}
