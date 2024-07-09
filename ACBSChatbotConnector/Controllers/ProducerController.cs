using ACBSChatbotConnector.Models;
using ACBSChatbotConnector.Models.Request;
using ACBSChatbotConnector.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

namespace ACBSChatbotConnector.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProducerController : ControllerBase
    {
        private readonly IProducerService _producerService;
        public ProducerController(IProducerService ProducerService)
        {
            _producerService = ProducerService;
        }
        [HttpPost]
        [Route("Data")]
        public async Task<IActionResult> PostDataAsync([FromBody] SendMessageRequest message)
        {
            try
            {
                string jsonMessage = JsonConvert.SerializeObject(message);

                Log.Information($"Send message test to Kafka. {jsonMessage}");

                await _producerService.ProduceMessageKafkaAsyncs(message);
                return Ok("Data has been sent to Kafka.");
            }
            catch (Exception ex)
            {
                Log.Error($"{ex}");
                return StatusCode(500, "An error occurred while sending data to Kafka.");
            }
        }
    }
}