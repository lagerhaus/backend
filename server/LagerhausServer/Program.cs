using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LagerhausServer
{
    public class Program
    {
        const string ENV_VAR_PREFIX = "LHAUS_";

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) {
            var backendPort = Environment.GetEnvironmentVariable($"{ENV_VAR_PREFIX}BACKEND_PORT") ?? "5001";
            return WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) => {
                    config.AddEnvironmentVariables(ENV_VAR_PREFIX);
                })
                .UseStartup<Startup>()
                .UseUrls($"http://*:{backendPort}");
        }
    }
}
