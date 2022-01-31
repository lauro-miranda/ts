using LM.Responses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TSDelivery.Accounts.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class Controller : ControllerBase
    {
        protected async Task<IActionResult> WithResponseAsync<TResponseMessage>(Func<Task<Response<TResponseMessage>>> func)
        {
            var response = await func.Invoke();

            if (!response.HasError)
                return Ok(response);

            if (response.Messages.Any(m => m.Type == MessageType.BusinessError))
                return BadRequest(response);

            return StatusCode(500, response);
        }

        protected async Task<IActionResult> WithResponseAsync(Func<Task<Response>> func)
        {
            var response = await func.Invoke();

            if (!response.HasError)
                return Ok(response);

            if (response.Messages.Any(m => m.Type == MessageType.BusinessError))
                return BadRequest(response);

            return StatusCode(500, response);
        }
    }
}