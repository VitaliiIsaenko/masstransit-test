using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace WebApplication
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
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "WebApplication", Version = "v1"});
            });
            services.AddMassTransit(x =>
            {
                x.UsingAmazonSqs((context, conf) =>
                {
                    conf.Host("eu-central-1", h =>
                    {
                        h.AccessKey("");
                        h.SecretKey("");

                        // following are OPTIONAL

                        // specify a scope for all queues
                        // will be in the beginning of the topic name with following _ (underscore)
                        // also an application name will be part of the topic name with following - (hyphen)
                        // In total topic name: dev_WebApplication-TestData (class name in the end)
                        h.Scope("dev");

                        // scope topics as well
                        h.EnableScopedTopics();
                    });
                    //important to setup the receivers after the Host, otherwise NullReferenceException
                });
            });
            services.AddMassTransitHostedService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApplication v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}