namespace RabbitMQ.Issuers.Api
{
    public class RabbitMQSettings
    {
        public string BaseURL { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string OrderDeliveryPositionChangedExchangeName { get; set; }

        public string OrderDeliveryStatusChangedExchangeName { get; set; }

        public string OrderCreatedExchangeName { get; set; }

        public string UserDetailsExchangeName { get; set; }
    }
}
