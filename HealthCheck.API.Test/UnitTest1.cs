using HealthCheck.API.Controllers;
using HealthCheck.Helper;
using HealthCheck.Helper.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Web.Http.Results;

namespace HealthCheck.API.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var controller = new HealthCheckController();
            var result = controller.Get() as OkNegotiatedContentResult<HealthCheckResponse>;

            Assert.IsFalse(result.Content.errors.Any());
        }
    }
}
