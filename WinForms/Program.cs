using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForms
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 日志记录，使用serilog接管全局日志
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File($"logs\\{Application.ProductName}[{Application.ProductVersion}].log", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            Log.Information("开始运行");

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            // 完成后清理日志
            Log.CloseAndFlush();
        }
    }
}
