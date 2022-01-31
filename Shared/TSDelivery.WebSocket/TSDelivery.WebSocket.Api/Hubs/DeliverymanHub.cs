using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using TSDelivery.WebSocket.Api.Domain;
using TSDelivery.WebSocket.Api.Domain.Models;
using TSDelivery.WebSocket.Api.Hubs.Contracts;
using TSDelivery.WebSocket.Api.Repositories.Contracts;

namespace TSDelivery.WebSocket.Api.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DeliverymanHub : Hub<IDeliverymanHub>
    {
        IUser User { get; }

        IDeliverymanRepository DeliverymanRepository { get; }

        public DeliverymanHub(IUser user
            , IDeliverymanRepository deliverymanRepository)
        {
            User = user;
            DeliverymanRepository = deliverymanRepository;
        }

        public override async Task OnConnectedAsync()
            => await DeliverymanRepository.CreateOrUpdateAsync(new Deliveryman(User.Identification, Context.ConnectionId));

        public override async Task OnDisconnectedAsync(Exception exception)
            => await DeliverymanRepository.DeleteAsync(Context.ConnectionId);
    }
}