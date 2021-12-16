using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("message")]
    public class MessageController : ControllerBase
    {
       private readonly ILogger<MessageController> _logger;
       private readonly IPublishEndpoint _publishEndpoint;

       public MessageController(ILogger<MessageController> logger, IPublishEndpoint publishEndpoint)
       {
           _logger = logger;
           _publishEndpoint = publishEndpoint;
       }

        [HttpPost]
        [Route("{id}")]
        public async void Publish(int id)
        {
            await _publishEndpoint.Publish<TestData>(
                new
                {
                    Id = id,
                    Name = "hey there"
                });
        }
    }
}