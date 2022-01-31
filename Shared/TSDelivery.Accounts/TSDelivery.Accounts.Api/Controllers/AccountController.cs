using LM.Responses;
using LM.Responses.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TSDelivery.Accounts.Api.Domain;
using TSDelivery.Accounts.Api.Domain.Services.Contracts;
using TSDelivery.Accounts.Api.Messages.Requests;
using TSDelivery.Accounts.Api.Messages.Responses;

namespace TSDelivery.Accounts.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class AccountController : Controller
    {
        IUserService UserService { get; }

        IOptions<JWTSettings> JWTSettings { get; }

        public AccountController(IUserService userService
            , IOptions<JWTSettings> jwtSettings)
        {
            UserService = userService;
            JWTSettings = jwtSettings;
        }

        [HttpPost, Route("")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateUserRequestMessage requestMessage)
           => await WithResponseAsync(() => UserService.CreateAsync(requestMessage));

        [HttpPost, Route("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequestMessage requestMessage)
        {
            var response = Response<LoginResponseMessage>.Create();

            var loginResponse = await UserService.LoginAsync(requestMessage);

            if (loginResponse.HasError)
                return StatusCode(401, response.WithMessages(loginResponse.Messages));
            return GenerateResponseAsync(response, loginResponse);
        }

        IActionResult GenerateResponseAsync(Response<LoginResponseMessage> response, Response<User> loginResponse)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new List<Claim> { new Claim(nameof(loginResponse.Data.Value.Identification), loginResponse.Data.Value.Identification) }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JWTSettings.Value.Secret)), SecurityAlgorithms.HmacSha256Signature)
            });

            response.Data.Value = new LoginResponseMessage
            {
                Identification = loginResponse.Data.Value.Identification,
                Name = loginResponse.Data.Value.Name,
                Token = tokenHandler.WriteToken(token)
            };

            return Ok(response);
        }
    }
}