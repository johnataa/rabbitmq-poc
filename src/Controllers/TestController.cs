using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RabbitMQPoc.Infra.RabbitMQ.Contracts;

namespace RabbitMQPoc.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IMainProducer _producer;
        public TestController(IMainProducer producer)
        {
            _producer = producer;
        }

        [HttpGet("send-message")]
        public void SendMessage(string message)
        {
            _producer.SendMessage(message);
        }

        [HttpGet("stress")]
        public void StressTest(int quantity)
        {
            foreach (var i in Enumerable.Range(0, quantity))
            {
                _producer.SendMessage($"Message {i}");
            }
        }
    }
}