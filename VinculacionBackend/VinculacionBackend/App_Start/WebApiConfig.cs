using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using VinculacionBackend.ActionFilters;
using VinculacionBackend.Security.BasicAuthentication;

namespace VinculacionBackend
{
    public static class WebApiConfig
    {

        public static void Register(HttpConfiguration config)
        {

            config.EnableCors();
           // config.Services.Replace(typeof(IExceptionHandler), new MyExceptionHandler());
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