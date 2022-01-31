using LM.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TSDelivery.Deliverymans.SiloHost.Controllers
{
    public interface IController { }

    [ApiController, Route("api/[controller]")]
    public class Controller<TController> : ControllerBase
        where TController : IController
    {
        ILogger<TController> Logger { get; }

        protected Controller(ILogger<TController> logger)
        {
            Logger = logger;
        }

        protected async Task<IActionResult> WithResponseAsync<TResponseMessage>(Func<Task<Response<TResponseMessage>>> func)
        {
            Logger.LogInformation(func.Method.Name);

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

        protected IActionResult WithResponseAsync(Func<Response> func)
        {
            var response = func.Invoke();

            if (!response.HasError)
                return Ok(response);

            if (response.Messages.Any(m => m.Type == MessageType.BusinessError))
                return BadRequest(response);

            return StatusCode(500, response);
        }
    }
}