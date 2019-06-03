using HealthCheck.API.Controllers;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebApi.StructureMap;

namespace HealthCheck.API
{
    public static class StructureMapConfig
    {
        public static class IoC
        {
            public static IContainer Container { get; set; }

            static IoC()
            {
                Container = new StructureMap.Container();
            }
        }
        public static void Configure()
        {
            IoC.Container.Configure((cfg) =>
               {
                   cfg.For<ISomeInterfaceNotImplemented>().Use<SomeImplementation>();

               });
        }
    }
}