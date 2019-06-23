using System;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

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
            var backendUrl = GetEnvBackendUrl() ?? "http://*:5001";
            System.Console.WriteLine($"Using URL: {backendUrl}");

            return WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) => {
                    config.AddEnvironmentVariables(ENV_VAR_PREFIX);
                })
                .UseStartup<Startup>()
                .UseUrls(backendUrl);
        }

        private static string GetEnvBackendUrl() {
            var envVar = Environment.GetEnvironmentVariable($"{ENV_VAR_PREFIX}BACKEND_URL");
            if(envVar == null)
                return null;
            
            return Regex.Replace(envVar, "(http(?:s)?://[\\w.]+(?::\\d+)?)/.*", "$1");  // Remove anything after port
        }
    }
}
