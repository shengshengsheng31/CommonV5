using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonV5
{
    public class KafkaHelper
    {
        public static void KafkaConsumer(string bootstrapserver, string toppicName, string groupId,Action<string> MessageHandler)
        {
            // kafka配置
            ConsumerConfig conf = new ConsumerConfig
            {
                GroupId = groupId,
                BootstrapServers = bootstrapserver,
                AutoOffsetReset = AutoOffsetReset.Latest,//初始状态读取最新数据
                EnableAutoCommit = true,//自动提交偏移
                EnableAutoOffsetStore = false,//不阻塞的commit
                StatisticsIntervalMs = 5000,
                SessionTimeoutMs = 6000,
                EnablePartitionEof = true
            };
            // 监听
            using (IConsumer<Ignore, string> consumer = new ConsumerBuilder<Ignore, string>(conf).Build())
            {
                consumer.Subscribe(toppicName);
                int partitionCount = consumer.Assignment.Count;
                for (int i = 0; i < partitionCount; i++)
                {
                    // 从最新处开始消费
                    TopicPartition topicPartition = new TopicPartition(toppicName, i);
                    consumer.Seek(new TopicPartitionOffset(topicPartition, consumer.GetWatermarkOffsets(topicPartition).High));

                }

                while (true)
                {
                    ConsumeResult<Ignore, string> consumeResult = consumer.Consume();
                    if (consumeResult.Message!= null)
                    {
                        try
                        {
                            Console.WriteLine($"partition：{consumeResult.Partition}-offset：{consumeResult.Offset}-range:{consumer.GetWatermarkOffsets(new TopicPartition(toppicName,consumeResult.Partition))}");
                            MessageHandler(consumeResult.Message.Value);
                        }
                        catch (Exception ex)
                        {

                            MessageHandler(ex.Message);
                        }
                        finally
                        {
                            consumer.StoreOffset(consumeResult);
                        }
                    }
                }
            }
        }
    }
}
