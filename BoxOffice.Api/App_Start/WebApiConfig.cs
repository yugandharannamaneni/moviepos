using BoxOffice.Api.CustomFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace BoxOffice.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();
            //config.MessageHandlers.Add(new ResponseHandler());
            //config.Filters.Add(new ServiceExceptionFilterAttribute());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
