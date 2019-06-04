using HealthCheck.Helper.Models;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthCheck.Helper
{
    public class HealthCheckHelper
    {
        public static HealthReportEntry TestaControllersInjection(string nomeAPI, IContainer container)
        {
            var mensagens = new List<string>();

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            var controllers = Assembly.Load(nomeAPI)
                                        .GetTypes()
                                        .Where(x => !string.IsNullOrEmpty(x.Namespace))
                                        .Where(x => x.IsClass)
                                        .Where(x => x.Name.Contains("Controller") && !x.Name.Contains("HealthCheck"))
                                        .ToList();
            
            foreach (var controllerType in controllers)
            {
                try
                {
                    container.GetInstance(controllerType);
                }
                catch (Exception ex)
                {
                    mensagens.Add($"Erro ao instanciar controller: {controllerType.Name}; {ex.Message}");
                }
            }
            stopwatch.Stop();

            var entry = new HealthReportEntry(
                       mensagens.Any() ? HealthStatus.Unhealthy : HealthStatus.Healthy,
                       "", TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds), null, null);
            
            return entry;
        }

        public static List<ErrorDto> TestaConnectionString(string connectionString, string sql)
        {
            var errors = new List<ErrorDto>();
            var mensagens = new List<string>();
          
            //try
            //{
            //    using (var connection = new OracleConnection(connectionString))
            //    {
            //        await connection.OpenAsync();
            //        using (var command = connection.CreateCommand())
            //        {
            //            command.CommandText = sql;
            //            await command.ExecuteScalarAsync();
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    mensagens.Add(ex.Message);
            //}

            if (mensagens.Any())
                errors.Add(new ErrorDto()
                {
                    key = "sql",
                    value = string.Join(" / ", mensagens)
                });

            return errors;
        }
    }

}
