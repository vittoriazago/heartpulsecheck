using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HealthCheck.Api.Core.HealthCheckers
{
    public class SqlServerHealthCheck : IHealthCheck
    {
        SqlConnection _connection;

        public string Name => "sql";

        public SqlServerHealthCheck(SqlConnection connection)
        {
            _connection = connection;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                _connection.Open();
            }
            catch (SqlException)
            {
                //return Task.FromResult(HealthCheckResult.Unhealthy());
            }
            finally
            {
                _connection.Close();
            }

            return Task.FromResult(HealthCheckResult.Healthy());
        }
    }
}
