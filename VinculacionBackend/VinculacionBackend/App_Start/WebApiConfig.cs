using System.Web.Http;

namespace VinculacionBackend
{
    public static class WebApiConfig
    {

        public static void Register(HttpConfiguration config)
        {

            config.EnableCors();

            config.Filters.Add(new CustomAuthorizeAttribute());
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}