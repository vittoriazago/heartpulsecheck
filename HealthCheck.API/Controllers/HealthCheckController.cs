using HealthCheck.Helper;
using HealthCheck.Helper.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Web.Http;
using static HealthCheck.API.StructureMapConfig;

namespace HealthCheck.API.Controllers
{
    [RoutePrefix("hc")]
    public class HealthCheckController : ApiController
    {
        public IHttpActionResult Get()
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            var entries = new Dictionary<string, HealthReportEntry>();
            entries.Add("DI Controllers", HealthCheckHelper.TestaControllersInjection("HealthCheck.API", IoC.Container));

            stopwatch.Stop();

            var response = UIHealthReport.CreateFrom(
                new HealthReport(entries, TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds)));

            if (response.Status == UIHealthStatus.Healthy)
                return Ok(response);
            else
                return Content((System.Net.HttpStatusCode)418, response);

        }
    }

}
