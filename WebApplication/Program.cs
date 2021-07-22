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
            // ��־��¼��ʹ��serilog�ӹ�ȫ����־
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(Path.Combine("Logs", @"serilog.log"), rollingInterval: RollingInterval.Day)
                .MinimumLevel.Debug()
                .CreateLogger();
            Log.Information("��ʼ����");
            Console.WriteLine("��ʼ����");

            CreateHostBuilder(args).Build().Run();

            // ��ɺ�������־
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
