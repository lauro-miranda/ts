using System.Runtime.Serialization;

namespace TSDelivery.WebSocket.Api.Messages
{
    [DataContract]
    public class MessageRequestMessage
    {
        [DataMember]
        public string Identification { get; set; }

        [DataMember]
        public string Message { get; set; }
    }
}