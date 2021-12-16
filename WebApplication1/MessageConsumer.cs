using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using WebApplication;

namespace WebApplication1
{
    public class MessageConsumer : IConsumer<TestData>
    {
        private readonly ILogger<MessageConsumer> _logger;
        public MessageConsumer(ILogger<MessageConsumer> logger)
        {
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<TestData> context)
        {
            _logger.LogCritical($"{context.Message.Id};{context.Message.Name}");
        }
    }
}