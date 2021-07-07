using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using Serilog;
using System;
using System.Text;
using System.Threading;

namespace CommonV5
{
    public class MqHelper
    {
        //MqttFactory factory = new MqttFactory();
        static IMqttClient mqttClient= new MqttFactory().CreateMqttClient();

        /// <summary>
        /// mqtt消费
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="ip"></param>
        /// <param name="topic"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="cts"></param>
        /// <param name="MessageHandler"></param>
        public static void MqConsumer(string clientId, string ip, string topic, string userName, string password, CancellationTokenSource cts, Action<string> MessageHandler)
        {
            IMqttClientOptions options;
            if (password == null || password == "")
            {
                options = new MqttClientOptionsBuilder()
                    .WithClientId(clientId)
                    .WithTcpServer(ip)
                    .Build();
            }
            else
            {
                options = new MqttClientOptionsBuilder()
                    .WithClientId(clientId)
                    .WithTcpServer(ip)
                    .WithCredentials(userName, password)
                    .Build();
            }

            // 连接到mq server触发事件
            mqttClient.UseConnectedHandler(async e =>
            {
                Log.Information("连接服务器-ok");
                await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(topic).Build());
                Log.Information("订阅-ok");
            });

            // 收到消息触发事件
            mqttClient.UseApplicationMessageReceivedHandler(async e =>
            {
                string message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                var a= await mqttClient.UnsubscribeAsync(topic);
                // 测试取消订阅消息
                MessageHandler(e.ApplicationMessage.Topic);
                //Console.WriteLine($"{message}---{e.ApplicationMessage.Topic}");
            });

            // 断开连接或无法连接触发事件
            mqttClient.UseDisconnectedHandler(e =>
            {
                Log.Error("断开连接-ok");
                // 进行重连
                if (!cts.IsCancellationRequested)
                {
                    mqttClient.ConnectAsync(options, cts.Token);
                }
            });

            try
            {
                mqttClient.ConnectAsync(options, cts.Token);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                throw;
            }
        }
        public static void DisposeMq()
        {
            mqttClient.Dispose();
        }

        // 配置

        // mq生产
    }
    
}
