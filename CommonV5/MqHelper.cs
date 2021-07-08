using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using Serilog;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommonV5
{
    public class MqHelper
    {
        private IMqttClient _mqttClient;
        private IMqttClientOptions _mqttClientOptions;
        
        /// <summary>
        /// 实例化后连接mq
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="ip"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public  MqHelper(string clientId,string ip,string userName,string password)
        {
            _mqttClient = new MqttFactory().CreateMqttClient();
            MqttClientOptionsBuilder mqttClientOptionsBuilder = new MqttClientOptionsBuilder()
                .WithTcpServer(ip)
                .WithClientId(clientId);
            if (password != "" && password != null)
            {
                mqttClientOptionsBuilder.WithCredentials(userName, password);
            }
            _mqttClientOptions = mqttClientOptionsBuilder.Build();
            // 连接到mq server触发事件
            _mqttClient.UseConnectedHandler(e =>
            {
                Log.Information("连接服务器-ok");
            });
            // 断开连接或无法连接触发事件
            _mqttClient.UseDisconnectedHandler(e =>
            {
                Log.Error($"断开连接{e.Exception} {e.Reason}");
                // 进行重连
                _mqttClient.ConnectAsync(_mqttClientOptions);
            });
            try
            {
                _mqttClient.ConnectAsync(_mqttClientOptions).Wait();
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 消费
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="MessageHandler"></param>
        public async Task<bool> Subscribe(string topic, Action<string> MessageHandler)
        {
            // 收到消息触发事件
            _mqttClient.UseApplicationMessageReceivedHandler(e =>
            {
                string message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                MessageHandler($"{e.ApplicationMessage.Topic}:{message}");
            });

            if (_mqttClient.IsConnected)
            {
                try
                {
                    await _mqttClient.SubscribeAsync(topic);
                    Log.Information($"订阅{topic}-ok");
                    return true;
                }
                catch (Exception ex)
                {
                    Log.Error(ex, ex.Message);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 取消topic订阅
        /// </summary>
        /// <param name="topic"></param>
        public async Task<bool> UnSubscribe(string topic)
        {
            try
            {
                await _mqttClient.UnsubscribeAsync(topic);
                Log.Information($"取消订阅{topic}");
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return false;
            }    
        }

        // mq生产
        public async Task<bool> MqProducer()
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic("MyTopic")
                .WithPayload("Hello World")
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();

            await _mqttClient.PublishAsync(message, CancellationToken.None);
            return false;
        }

        
    }
    
}
