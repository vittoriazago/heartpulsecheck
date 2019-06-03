using HealthCheck.Helper;
using HealthCheck.Helper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;

namespace HealthCheck.API.Controllers
{
    [RoutePrefix("hc")]
    public class HealthCheckController : ApiController
    {
        public IHttpActionResult Get()
        {
            var errors = new List<ErrorDto>();
            errors.AddRange(HealthCheckHelper.TestaControllersInjection("HealthCheck.API"));

            return Ok(new HealthCheckResponse
            {
                status = "Health",
                version = Assembly.GetExecutingAssembly().GetName().Version,
                errors = errors

            });
        }
    }
    
}
