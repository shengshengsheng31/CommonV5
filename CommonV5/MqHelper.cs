using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonV5
{
    public class MqHelper
    {
        public static void MqConsumer(string clientId,string ip,string topic,string userName,string password)
        {
            MqttFactory factory = new MqttFactory();
            IMqttClient mqttClient = factory.CreateMqttClient();
            IMqttClientOptions options = new MqttClientOptionsBuilder()
                .WithClientId(clientId)
                .WithTcpServer(ip)
                //.WithCredentials(userName,password)
                .Build();

            // 连接到mq server触发事件
            mqttClient.UseConnectedHandler(async e =>
            {
                Console.WriteLine(e.AuthenticateResult);
                if (e.AuthenticateResult.ResultCode== MqttClientConnectResultCode.Success)
                {
                    Log.Information("连接服务器-ok");
                    await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(topic).Build());
                    Log.Information("订阅-ok");
                }
                else
                {
                    Log.Error("连接失败");
                }
                
            });

            // 收到消息触发事件
            mqttClient.UseApplicationMessageReceivedHandler(e =>
            {
                Console.WriteLine(e);
            });

            // 断开连接触发事件
            mqttClient.UseDisconnectedHandler(e =>
            {
                Console.WriteLine(e);
            });

            mqttClient.ConnectAsync(options);
        }
    }
}
