using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using TSDelivery.WebSocket.Api.Hubs;
using TSDelivery.WebSocket.Api.Hubs.Contracts;
using TSDelivery.WebSocket.Api.Messages;
using TSDelivery.WebSocket.Api.Repositories.Contracts;

namespace TSDelivery.WebSocket.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class DeliverymanController : Controller
    {
        IHubContext<DeliverymanHub, IDeliverymanHub> Hub { get; }

        IDeliverymanRepository Repository { get; }

        public DeliverymanController(IHubContext<DeliverymanHub, IDeliverymanHub> hub
            , IDeliverymanRepository repository)
        {
            Hub = hub;
            Repository = repository;
        }

        [HttpPost, Route("message")]
        public async Task<IActionResult> SendMessageAsync([FromBody] MessageRequestMessage requestMessage)
        {
            var Deliveryman = await Repository.FindAsync(requestMessage.Identification);

            if (!Deliveryman.HasValue)
                return NoContent();

            await Hub.Clients.Client(Deliveryman.Value.ConnectionId).OnMessage(requestMessage.Message);

            return Accepted();
        }
    }
}