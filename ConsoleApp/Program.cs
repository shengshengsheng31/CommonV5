using CommonV5;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static int taskCount = 0;
        static  void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //ThreadTest().GetAwaiter().GetResult();
            //Console.WriteLine("------------------1---------------");
            //ThreadTest().GetAwaiter();
            //Console.WriteLine("------------------2---------------");

            //string bootstrapserver = "49.234.93.236:9092";
            //string toppicName = "test";
            //string groupId = "sheng31";
            string bootstrapserver = "32.96.1.69:9092";
            string toppicName = "BAYONET_VEHICLEPASS_JSON_TOPIC";
            string groupId = "sheng31";
            KafkaHelper.KafkaConsumer(bootstrapserver, toppicName, groupId, value=> {
                try
                {
                    VehicleDto.Root vehicle = JsonSerializer.Deserialize<VehicleDto.Root>(value);
                    
                    string plateNo = vehicle.vehicleRcogResult[0].target[0].vehicle.plateNo.value;
                    string passTime = vehicle.vehicleRcogResult[0].targetAttrs.passTime;
                    string crossingName = vehicle.vehicleRcogResult[0].targetAttrs.cameraName;
                    Console.WriteLine($"passTime:{passTime}-plateNo:{plateNo}-crossingName{crossingName}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(value.Substring(0,100));
                    
                }
            });
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
