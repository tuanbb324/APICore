using Confluent.Kafka;
using Newtonsoft.Json;
using Confluent.Kafka.Admin;
using ACBSChatbotConnector.Services;
using ACBSChatbotConnector.Models;
using ACBSChatbotConnector.Models.Request;

public class ProducerService : IProducerService
{
    private readonly IProducer<string, string> _producer;
    private readonly string _topicName;
    public ProducerService()
    {
        var _config = new ProducerConfig
        {
            BootstrapServers = "kafka:9092",
        };
        _topicName = "ACBS_Message_Sync";
        _producer = new ProducerBuilder<string, string>(_config).Build();

        using (var adminClient = new AdminClientBuilder(_config).Build())
        {
            var topicExists = adminClient.GetMetadata(_topicName, TimeSpan.FromSeconds(10));
            if (topicExists.Topics.Count == 0)
            {
                adminClient.CreateTopicsAsync(new TopicSpecification[] {
                        new TopicSpecification { Name = _topicName, NumPartitions = 1, ReplicationFactor = 1 }
                    }).Wait();
            }
        }
    }
    public async Task ProduceMessageAsyncs(ChatMessage message)
    {
        string jsonMessage = JsonConvert.SerializeObject(message);

        var kafkaMessage = new Message<string, string>
        {
            Key = Guid.NewGuid().ToString(),
            Value = jsonMessage
        };
        await _producer.ProduceAsync(_topicName, kafkaMessage);
    }
    public async Task ProduceMessageKafkaAsyncs(SendMessageRequest message)
    {
        string jsonMessage = JsonConvert.SerializeObject(message);

        var kafkaMessage = new Message<string, string>
        {
            Key = Guid.NewGuid().ToString(),
            Value = jsonMessage
        };
        await _producer.ProduceAsync(_topicName, kafkaMessage);
    }
}
