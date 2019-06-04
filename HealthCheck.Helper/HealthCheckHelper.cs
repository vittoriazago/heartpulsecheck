using HealthCheck.Helper.Models;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HealthCheck.Helper
{
    public class HealthCheckHelper
    {
        public static Microsoft.Extensions.Diagnostics.HealthChecks.HealthReportEntry TestaControllersInjection(string nomeAPI, IContainer container)
        {
            var errors = new List<ErrorDto>();
            var mensagens = new List<string>();
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
            var teste = Microsoft.Extensions.Diagnostics.HealthChecks.HealthReportEntry();
            if (mensagens.Any())
                errors.Add(new ErrorDto()
                {
                    key = "DI Controllers",
                    value = string.Join(" / ", mensagens)
                });

            return errors;
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
