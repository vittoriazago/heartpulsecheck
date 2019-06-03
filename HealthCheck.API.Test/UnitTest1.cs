using HealthCheck.API.Controllers;
using HealthCheck.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace HealthCheck.API.Test
{
    [TestClass]
    public class UnitTest1
    {
        //private readonly ApiServer _server;
        //private readonly HttpClientWrapper _client;

        [TestMethod]
        public void TestMethod1()
        {
            var erros = HealthCheckHelper.TestaControllersInjection("HealthCheck.API");
            Assert.IsFalse(erros.Any());
        }
    }
}
