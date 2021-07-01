using System;
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
            ThreadTest().GetAwaiter().GetResult();
            Console.WriteLine("------------------1---------------");
            ThreadTest().GetAwaiter();
            Console.WriteLine("------------------2---------------");

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
