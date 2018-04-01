using System.Configuration;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ExceptionHandling;
using UserApi.ExceptionHandling;
using UserApi.Messaging;
using UserApi.Repos;

namespace UserApi
{
    public static class StartUp
    {
        public static void Configure(HttpConfiguration config)
        {
            var domains = ConfigurationManager.AppSettings.Get("CorsDomains");
            if (!string.IsNullOrWhiteSpace(domains)) config.EnableCors(new EnableCorsAttribute(domains, "*", "GET,POST,DELETE"));

            ConfigureExceptionHandling(config);

            //UserRepo.Configure();
            //MessageBus.Initialize();

            ConfigureDependencyInjection();

            ConfigureRoutes(config);
        }

        public static void CleanUp()
        {
            UnityConfig.Container.Dispose();
        }

        private static void ConfigureDependencyInjection()
        {
            GlobalConfiguration.Configuration.DependencyResolver = UnityConfig.Resolver;
        }

        private static void ConfigureRoutes(HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        private static void ConfigureExceptionHandling(HttpConfiguration config)
        {
            config.Services.Replace(typeof(IExceptionHandler), new BaseExceptionHandler());
            config.Services.Replace(typeof(IExceptionLogger), new TraceExceptionLogger());
        }
    }
}
