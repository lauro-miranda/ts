using System;

namespace TSDelivery.WebSocket.Api.Domain.Models
{
    public class Deliveryman
    {
        public Deliveryman(string identification, string connectionId)
        {
            Id = Guid.NewGuid();
            Identification = identification;
            ConnectionId = connectionId;
        }

        public Guid Id { get; set; }

        public string Identification { get; set; }

        public string ConnectionId { get; set; }
    }
}