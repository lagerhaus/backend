﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lagerhaus.Processors;
using Lagerhaus.Validation;
using LagerhausDb;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LagerhausServer
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContext<LagerhausContext>(options =>
                options.UseNpgsql(
                    Configuration.GetValue("CONNECTION_STRING", Configuration.GetConnectionString("LagerhausContext"))
                )
            );

            services.AddScoped<RegionsValidation>();
            services.AddScoped<RegionsProcessor>();

            services.AddScoped<WeatherValidation>();
            // services.AddScoped<WeatherProcessor>();

            services.AddScoped<BatchesValidation>();
            services.AddScoped<BatchesProcessor>();
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
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseCors(policy => policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
            );
            app.UseMvc();
        }
    }
}
