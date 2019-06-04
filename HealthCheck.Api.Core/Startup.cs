using HealthCheck.Api.Core.HealthCheckers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace HealthCheck.Api.Core
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSingleton<SqlServerHealthCheck>();
            services.AddSingleton<SqlConnection>(s => new SqlConnection("Server=.;Trusted_Connection=True;Database=PONTOFIDELIDADE"));
            services.AddHealthChecks()
                    .AddCheck<SqlServerHealthCheck>("sql")
              ;


            services.AddCors(
              options => options.AddPolicy("AllowCors",
              builder =>
              {
                  builder
                      .AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
              })
          );

            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("AllowCors"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors("AllowCors");

            var options = new HealthCheckOptions();
            options.Predicate = _ => true;
            options.AllowCachingResponses = false;
            //options.ResultStatusCodes[HealthStatus.Unhealthy] = 418;
            //options.ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse;
            options.ResponseWriter = async (c, r) =>
            {
                var settings = new JsonSerializerSettings()
                {
                    ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                };
                settings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());

                var uiReport = Helper.Models.UIHealthReport
                    .CreateFrom(r);

                var response = JsonConvert.SerializeObject(uiReport, settings);
                await c.Response.WriteAsync(response);
            };
            app.UseHealthChecks("/hc", options);

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }

}
