using CommonV5;
using Serilog;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static int taskCount = 0;
        static void Main(string[] args)
        {
            // 日志记录，使用serilog接管全局日志
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File($"logs\\Console.log", rollingInterval: RollingInterval.Day)
                .MinimumLevel.Debug()
                .CreateLogger();
            Log.Information("开始运行");

            Console.WriteLine("Hello World!");

            string bootstrapserver = "49.234.93.236:9092";
            string topicName = "test";
            string groupId = "sheng31";
            //string bootstrapserver = "32.96.1.69:9092";
            //string toppicName = "BAYONET_VEHICLEPASS_JSON_TOPIC";
            //string groupId = "sheng31";

            #region kafka消费
            //CancellationTokenSource cts = new CancellationTokenSource();
            //// 停止消费
            //Console.CancelKeyPress += (_, e) =>
            //{
            //    e.Cancel = true;
            //    cts.Cancel();
            //};
            //KafkaHelper.KafkaConsumer(bootstrapserver, toppicName, groupId, cts, true, value =>
            // {
            //     try
            //     {
            //         VehicleDto.Root vehicle = JsonSerializer.Deserialize<VehicleDto.Root>(value);
            //         string plateNo = vehicle.vehicleRcogResult[0].target[0].vehicle.plateNo.value;
            //         string passTime = vehicle.vehicleRcogResult[0].targetAttrs.passTime;
            //         string crossingName = vehicle.vehicleRcogResult[0].targetAttrs.cameraName;
            //         Console.WriteLine($"passTime:{passTime}-plateNo:{plateNo}-crossingName{crossingName}");
            //     }
            //    // 解析失败
            //    catch (Exception)
            //     {
            //         Console.WriteLine(value);
            //     }
            // }); 
            #endregion

            #region kafka生产
            //if( KafkaHelper.KafkaProducer(bootstrapserver, topicName, "a").Result)
            //{
            //    Console.WriteLine("ok");
            //}
            #endregion


            #region mq消费
            MqHelper.MqConsumer("sheng31", "test.ranye.net", "home/sensor/#", "sheng31", null);
            #endregion
            Console.ReadLine();
        }

        public static async Task ThreadTest()
        {
            taskCount++;
            await Task.Run(() =>
            {
                int i = 0;
                while (i < 100)
                {
                    i++;
                    string threadId = Thread.CurrentThread.ManagedThreadId.ToString();
                    Console.WriteLine($"{taskCount}-{i}-{threadId}");
                }
            });
        }
    }
}
