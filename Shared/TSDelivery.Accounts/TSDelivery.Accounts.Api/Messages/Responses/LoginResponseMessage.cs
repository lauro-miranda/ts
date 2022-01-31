using System.Runtime.Serialization;

namespace TSDelivery.Accounts.Api.Messages.Responses
{
    [DataContract]
    public class LoginResponseMessage
    {
        [DataMember]
        public string Identification { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Token { get; set; }
    }
}