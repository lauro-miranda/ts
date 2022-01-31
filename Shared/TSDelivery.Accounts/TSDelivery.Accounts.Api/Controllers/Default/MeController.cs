using Microsoft.AspNetCore.Mvc;

namespace TSDelivery.Accounts.Api.Controllers.Default
{
    [ApiController, Route("")]
    public class MeController : ControllerBase
    {
        [HttpGet, Route("")]
        public IActionResult Get() => Ok(new { name = "tsdelivery account", version = "1.0" });
    }
}