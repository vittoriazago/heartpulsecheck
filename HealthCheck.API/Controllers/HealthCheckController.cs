using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HealthCheck.API.Controllers
{
    [RoutePrefix("hc")]
    public class HealthCheckController : ApiController
    {
        // GET api/values
        public CheckModel Get()
        {
            return new CheckModel
            {
                status = "Health"
            };
        }
    }

    public class CheckModel
    {
        public string status { get; set; }
    }
}
