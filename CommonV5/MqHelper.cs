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
        public static void MqConsumer(string clientId,string ip,string topic,string userName,string password, CancellationTokenSource cts, Action<string> MessageHandler)
        {
            MqttFactory factory = new MqttFactory();
            IMqttClient mqttClient = factory.CreateMqttClient();
            IMqttClientOptions options;
            if (password == null||password=="")
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
            mqttClient.UseApplicationMessageReceivedHandler(e =>
            {
                string message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                MessageHandler(message);
                //Console.WriteLine($"{message}---{e.ApplicationMessage.Topic}");
            });

            // 断开连接或无法连接触发事件
            mqttClient.UseDisconnectedHandler(e =>
            {
                Log.Error("断开连接-ok");
                // 进行重连
                mqttClient.ConnectAsync(options,cts.Token);
            });

            try
            {
                mqttClient.ConnectAsync(options,cts.Token);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                throw;
            }
            
        }
    }
}
