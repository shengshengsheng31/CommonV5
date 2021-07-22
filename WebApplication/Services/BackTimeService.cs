using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebApplication.Services
{
    public class BackTimeService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Serilog.Log.Information(DateTime.Now.ToString("yyyyy-MM-dd HH:mm:ss"));
                Console.WriteLine(DateTime.Now.ToString("yyyyy-MM-dd HH:mm:ss"));
                await Task.Delay(2000);
            }
        }
    }
}
