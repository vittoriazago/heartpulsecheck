using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthCheck.API.Models
{
    public class Version
    {
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Build { get; set; }
        public int Revision { get; set; }
        public int MajorRevision { get; set; }
        public int MinorRevision { get; set; }
    }

    public class Error
    {
        public string key { get; set; }
        public string value { get; set; }
    }

    public class HealthCheckResponse
    {
        public string status { get; set; }
        public System.Version version { get; set; }
        public List<Error> errors { get; set; }
    }
}