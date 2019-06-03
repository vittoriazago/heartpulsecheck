using HealthCheck.API.Controllers;
using StructureMap;
using System.Web.Http;
using System.Web.Mvc;
using WebApi.StructureMap;

namespace HealthCheck.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            GlobalConfiguration.Configuration.UseStructureMap<DefaultRegistry>();
        }
    }
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            //Scan(
            //    scan =>
            //    {
            //        scan.TheCallingAssembly();
            //        scan.WithDefaultConventions();
            //    });

            For<ISomeInterfaceNotImplemented>().Use<SomeImplementation>();
        }
    }

}