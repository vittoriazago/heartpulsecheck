﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace HealthCheck
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

            services.AddHealthChecks();
            services.AddHealthChecksUI();
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

            var options = new HealthCheckOptions();
            options.ResultStatusCodes[HealthStatus.Unhealthy] = 418;
            options.Predicate = _ => true;
            options.AllowCachingResponses = false;
            //options.ResponseWriter = () => { };
            options.ResponseWriter = async (c, r) =>
            {
                c.Response.ContentType = "application/json";

                var result = JsonConvert.SerializeObject(new
                {
                    status = r.Status.ToString(),
                    version = Assembly.GetCallingAssembly().GetName().Version,
                    errors = r.Entries.Select(e => new { key = e.Key, value = e.Value.Status.ToString() })
                });
                await c.Response.WriteAsync(result);
            };
            app.UseHealthChecks("/hc", options);

            app.UseHealthChecksUI(config => config.UIPath = "/ui");
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
