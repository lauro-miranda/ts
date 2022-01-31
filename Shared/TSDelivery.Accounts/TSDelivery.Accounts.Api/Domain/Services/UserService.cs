using LM.Responses;
using LM.Responses.Extensions;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using TSDelivery.Accounts.Api.Domain.Repositories.Contracts;
using TSDelivery.Accounts.Api.Domain.Services.Contracts;
using TSDelivery.Accounts.Api.Messages.Requests;

namespace TSDelivery.Accounts.Api.Domain.Services
{
    public class UserService : IUserService
    {
        IUserRepository UserRepository { get; }

        IOptions<JWTSettings> JWTSettings { get; }

        public UserService(IUserRepository userRepository
            , IOptions<JWTSettings> jwtSettings)
        {
            UserRepository = userRepository;
            JWTSettings = jwtSettings;
        }

        public async Task<Response> CreateAsync(CreateUserRequestMessage requestMessage)
        {
            var response = Response<User>.Create();

            if (response.HasError)
                return response;

            var user = User.Create(requestMessage.Name
                , requestMessage.Identification
                , requestMessage.Password
                , JWTSettings.Value.Secret);

            if (user.HasError)
                return response.WithMessages(user.Messages);

            await UserRepository.CreateOrUpdateAsync(user);

            return response;
        }

        public async Task<Response<User>> LoginAsync(LoginRequestMessage requestMessage)
        {
            var response = Response<User>.Create();

            if (requestMessage == null) requestMessage = new LoginRequestMessage();

            var user = await UserRepository.FindAsync(requestMessage.Identification);

            if (!user.HasValue)
                return response.WithBusinessError(nameof(requestMessage.Password), "Usuário e/ou senha inválido(s).");

            if (response.WithMessages(user.Value.Login(requestMessage.Password, JWTSettings.Value.Secret).Messages).HasError)
                return response;

            return response.SetValue(user);
        }
    }
}