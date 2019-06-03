using HealthCheck.Helper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HealthCheck.Helper
{
    public class HealthCheckHelper
    {
        public static List<ErrorDto> TestaControllersInjection(string nomeAPI)
        {
            var errors = new List<ErrorDto>();
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
            if (mensagens.Any())
                errors.Add(new ErrorDto()
                {
                    key = "DI Controllers",
                    value = string.Join(" / ", mensagens)
                });

            return errors;
        }
    }
    
}
