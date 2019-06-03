using HealthCheck.Helper;
using HealthCheck.Helper.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using static HealthCheck.API.StructureMapConfig;

namespace HealthCheck.API.Controllers
{
    [RoutePrefix("hc")]
    public class HealthCheckController : ApiController
    {        
        public IHttpActionResult Get()
        {
            var errors = new List<ErrorDto>();
            errors.AddRange(HealthCheckHelper.TestaControllersInjection("HealthCheck.API", IoC.Container));

            return Ok(new HealthCheckResponse
            {
                status = errors.Any() ? HealthStatus.Unhealthy.ToString() : HealthStatus.Healthy.ToString(),
                version = Assembly.GetExecutingAssembly().GetName().Version,
                errors = errors

            });
        }
    }
    
}
