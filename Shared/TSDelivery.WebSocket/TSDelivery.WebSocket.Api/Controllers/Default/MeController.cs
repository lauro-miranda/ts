using Microsoft.AspNetCore.Mvc;

namespace TSDelivery.WebSocket.Api.Controllers.Default
{
    [ApiController, Route("")]
    public class MeController : Controller
    {
        [HttpGet, Route("")]
        public IActionResult Get() => Ok(new { name = "tsdelivery", version = "1.0" });
    }
}