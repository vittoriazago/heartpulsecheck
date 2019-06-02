using HealthCheck.API.Models;
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
            var errors = new List<Error>();
            errors.AddRange(TestaControllersInjection("HealthCheck.API"));

            return Ok(new HealthCheckResponse
            {
                status = "Health",
                version = Assembly.GetExecutingAssembly().GetName().Version,
                errors = errors

            });
        }
        public static List<Error> TestaControllersInjection(string nomeAPI)
        {
            var errors = new List<Error>();
            var mensagens = new List<string>();
            var controllers = Assembly.Load(nomeAPI)
                                        .GetTypes()
                                        .Where(x => !string.IsNullOrEmpty(x.Namespace))
                                        .Where(x => x.IsClass)
                                        .Where(x => x.Name.Contains("Controller"))
                                        .ToList();

            foreach (var controller in controllers)
            {
                try
                {
                    controller.GetConstructor(new Type[] { }).Invoke(new object[] { });
                }
                catch (Exception ex)
                {
                    mensagens.Add($"Erro ao instanciar controller: {controller.Name}; {ex.Message}");
                }
            }
            if (errors.Any())
                errors.Add(new Error()
                {
                    key = "DI Controllers",
                    value = string.Join(" / ", mensagens)
                });

            return errors;
        }
    }
    
}
