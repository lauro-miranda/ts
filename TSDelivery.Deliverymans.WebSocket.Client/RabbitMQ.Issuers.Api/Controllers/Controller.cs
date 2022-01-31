using EasyNetQ;
using Microsoft.AspNetCore.Mvc;
using System;

namespace RabbitMQ.Issuers.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class Controller : ControllerBase
    {
        protected IAdvancedBus ServiceBus { get; set; }

        public Controller(IAdvancedBus serviceBus)
        {
            ServiceBus = serviceBus ?? throw new ArgumentNullException(nameof(serviceBus));
        }
    }
}