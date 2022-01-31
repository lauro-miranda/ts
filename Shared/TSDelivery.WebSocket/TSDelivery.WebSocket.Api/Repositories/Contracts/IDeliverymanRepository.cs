using LM.Responses;
using System.Threading.Tasks;
using TSDelivery.WebSocket.Api.Domain.Models;

namespace TSDelivery.WebSocket.Api.Repositories.Contracts
{
    public interface IDeliverymanRepository
    {
        Task<Maybe<Deliveryman>> FindAsync(string identification);

        Task CreateOrUpdateAsync(Deliveryman Deliveryman);

        Task DeleteAsync(string identification);
    }
}