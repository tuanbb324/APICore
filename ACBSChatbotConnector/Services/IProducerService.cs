using ACBSChatbotConnector.Models;
using ACBSChatbotConnector.Models.Request;
using Newtonsoft.Json;

namespace ACBSChatbotConnector.Services
{
    public interface IProducerService
    {
        Task ProduceMessageAsyncs(ChatMessage message);
        Task ProduceMessageKafkaAsyncs(SendMessageRequest message);
    }
}
