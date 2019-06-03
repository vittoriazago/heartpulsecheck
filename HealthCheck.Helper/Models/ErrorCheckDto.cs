using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCheck.Helper.Models
{
    class ErrorCheckDto
    {
    }

    public class ErrorDto
    {
        public string key { get; set; }
        public string value { get; set; }
    }

    public class HealthCheckResponse
    {
        public string status { get; set; }
        public System.Version version { get; set; }
        public List<ErrorDto> errors { get; set; }
    }
}
