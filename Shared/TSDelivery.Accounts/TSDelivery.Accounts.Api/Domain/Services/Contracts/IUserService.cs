using LM.Responses;
using System.Threading.Tasks;
using TSDelivery.Accounts.Api.Messages.Requests;

namespace TSDelivery.Accounts.Api.Domain.Services.Contracts
{
    public interface IUserService
    {
        Task<Response> CreateAsync(CreateUserRequestMessage requestMessage);

        Task<Response<User>> LoginAsync(LoginRequestMessage requestMessage);
    }
}