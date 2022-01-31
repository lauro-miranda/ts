using System.Threading.Tasks;

namespace TSDelivery.WebSocket.Api.Hubs.Contracts
{
    public interface IDeliverymanHub
    {
        Task OnMessage(string message);
    }
}