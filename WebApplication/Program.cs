using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // 日志记录，使用serilog接管全局日志
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(Path.Combine("Logs", @"serilog.log"), rollingInterval: RollingInterval.Day)
                .MinimumLevel.Debug()
                .CreateLogger();
            Log.Information("开始运行");
            Console.WriteLine("开始运行");

            CreateHostBuilder(args).Build().Run();

            // 完成后清理日志
            Log.CloseAndFlush();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .UseStartup<Startup>();
                })
            .UseSerilog();
    }
}
