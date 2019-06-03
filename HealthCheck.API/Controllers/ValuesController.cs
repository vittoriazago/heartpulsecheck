using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HealthCheck.API.Controllers
{
    public class ValuesController : ApiController
    {
        ISomeInterfaceNotImplemented _someInterfaceNotImplemented;

        protected ValuesController()
        {

        }
        public ValuesController(ISomeInterfaceNotImplemented someInterfaceNotImplemented)
        {
            _someInterfaceNotImplemented = someInterfaceNotImplemented;
        }

        // GET api/values
        public IEnumerable<string> Get()
        {
            _someInterfaceNotImplemented.doSomething();
            return new string[] { "value1", "value2" };
        }
        
    }

    public interface ISomeInterfaceNotImplemented
    {
        void doSomething();
    }

    public class SomeImplementation : ISomeInterfaceNotImplemented
    {
        public void doSomething()
        {
            //throw new NotImplementedException();
        }
    }
}
