using Confluent.Kafka;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommonV5
{
    public class KafkaHelper
    {
        /// <summary>
        /// kafka消费
        /// </summary>
        /// <param name="bootstrapserver"></param>
        /// <param name="topicName"></param>
        /// <param name="groupId"></param>
        /// <param name="MessageHandler"></param>
        /// <param name="cts"></param>
        public static void KafkaConsumer(string bootstrapserver, string topicName, string groupId, CancellationTokenSource cts, bool isAlwaysLatest,Action<string> MessageHandler)
        {
            // kafka配置
            ConsumerConfig conf = new ConsumerConfig
            {
                GroupId = groupId,
                BootstrapServers = bootstrapserver,
                AutoOffsetReset = AutoOffsetReset.Latest,   //初始状态读取最新数据
                EnableAutoCommit = true,    //定期自动提交偏移
                EnableAutoOffsetStore = false   //不阻塞的commit
            };
            // 监听
            using (IConsumer<Ignore, string> consumer = new ConsumerBuilder<Ignore, string>(conf).Build())
            {
                consumer.Subscribe(topicName);
                while (consumer.Assignment.Count==0)
                {
                    // 等待一会，获取partition数量
                }
                // 始终从最新处开始消费
                if (isAlwaysLatest)
                {
                    for (int i = 0; i < consumer.Assignment.Count; i++)
                    {
                        TopicPartition topicPartition = new TopicPartition(topicName, i);
                        consumer.Seek(new TopicPartitionOffset(topicPartition, consumer.GetWatermarkOffsets(topicPartition).High));
                    }
                }
                Log.Information($"topic:{topicName} partitionCount:{consumer.Assignment.Count}");
                try
                {
                    // 持续监听至cancel
                    while (true)
                    {
                        ConsumeResult<Ignore, string> consumeResult = consumer.Consume(cts.Token);
                        if (consumeResult.Message != null)
                        {
                            try
                            {
                                //Console.WriteLine($"partition：{consumeResult.Partition} offset：{consumeResult.Offset} latest:{consumer.GetWatermarkOffsets(new TopicPartition(toppicName,consumeResult.Partition)).High}");
                                MessageHandler(consumeResult.Message.Value);
                            }
                            finally
                            {
                                // 手动提交offset
                                consumer.StoreOffset(consumeResult);
                            }
                        }
                    }
                }
                // 检测到终止结束监听
                catch (OperationCanceledException ex)
                {
                    Serilog.Log.Error(ex, ex.Message);
                    consumer.Close();
                }
            }
        }

        /// <summary>
        /// kafka生产
        /// </summary>
        /// <param name="bootstrapserver"></param>
        /// <param name="topicName"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task<bool> KafkaProducer(string bootstrapserver,string topicName,string data)
        {
            try
            {
                ProducerConfig conf = new ProducerConfig
                {
                    BootstrapServers = bootstrapserver,
                };
                using (IProducer<Null, string> producer = new ProducerBuilder<Null, string>(conf).Build())
                {
                    DeliveryResult<Null, string> result = await producer.ProduceAsync(topicName, new Message<Null, string> { Value = data });
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return false;
            }
            
        }

    }
}
