using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Orleans;
using System;
using System.Threading.Tasks;
using TSDelivery.Deliverymans.SiloHost.Grains;

namespace TSDelivery.Deliverymans.SiloHost.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class DeliverymanController : Controller<DeliverymanController>, IController
    {
        IClusterClient Client { get; }

        User CurrentUser { get; }

        public DeliverymanController(IClusterClient client
            , ILogger<DeliverymanController> logger
            , User user)
            : base(logger)
        {
            Client = client;
            CurrentUser = user;
        }

        [HttpGet, Route("{code}")]
        public async Task<IActionResult> GetAsync(Guid code)
        {
            var client = Client.GetGrain<IDeliverymanGrain>(code);

            if (client == null)
                return NoContent();

            return Ok(await client.Get());
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(DeliverymanRequest request)
        {
            var client = Client.GetGrain<IDeliverymanGrain>(request.Code);

            if (client == null)
                return NoContent();

            return Ok(await client.Create(request));
        }

        [HttpPost, Route("location/{code}")]
        public async Task<IActionResult> AddAsync(Guid code, [FromBody] Location request)
        {
            var deliveryman = Client.GetGrain<IDeliverymanGrain>(code);

            if (deliveryman == null)
                return NoContent();

            var location = Client.GetGrain<ILocationGrain>(request.Identification);

            if (location == null)
                return NoContent();

            return Ok(await location.Set(request));
        }

        [HttpGet, Route("location/{identification}")]
        public async Task<IActionResult> GetLocationAsync(string identification)
        {
            var location = Client.GetGrain<ILocationGrain>(identification);

            if (location == null)
                return NoContent();

            return Ok(await location.Get());
        }

        [HttpPost, Route("make-available")]
        public async Task<IActionResult> MakeAvailableAsync()
        {
            var location = Client.GetGrain<ILocationGrain>(CurrentUser.Identification);

            if (location == null)
                return NoContent();

            return Ok(await location.Get());
        }

        [HttpPost, Route("make-unavailable")]
        public async Task<IActionResult> MakeUnavailableAsync()
        {
            var location = Client.GetGrain<ILocationGrain>(CurrentUser.Identification);

            if (location == null)
                return NoContent();

            return Ok(await location.Get());
        }
    }
}