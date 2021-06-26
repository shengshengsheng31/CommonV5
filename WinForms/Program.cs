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
            // ��־��¼��ʹ��serilog�ӹ�ȫ����־
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File($"logs\\{Application.ProductName}[{Application.ProductVersion}].log", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            Log.Information("��ʼ����");

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            // ��ɺ�������־
            Log.CloseAndFlush();
        }
    }
}
