using HealthCheck.API.Controllers;
using HealthCheck.Helper;
using HealthCheck.Helper.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;
using System.Linq;
using System.Web.Http.Results;
using static HealthCheck.API.StructureMapConfig;

namespace HealthCheck.API.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            StructureMapConfig.Configure();

            var controller = new HealthCheckController();// IoC.Container);
            var result = controller.Get() as OkNegotiatedContentResult<HealthCheckResponse>;

            Assert.IsFalse(result.Content.errors.Any());
        }
    }
}
