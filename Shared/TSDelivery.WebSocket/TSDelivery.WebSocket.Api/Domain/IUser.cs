using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace TSDelivery.WebSocket.Api.Domain
{
    public interface IUser
    {
        string Identification { get; }

        bool HasUser { get; }
    }

    public class User : IUser
    {
        User() { }
        public User(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor == null)
                throw new ArgumentNullException(nameof(httpContextAccessor));

            Initialize(httpContextAccessor);
        }

        public string Identification { get; private set; }

        public bool HasUser => !string.IsNullOrEmpty(Identification);

        void Initialize(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor.HttpContext == null)
                return;

            var user = httpContextAccessor.HttpContext.User;

            if (user != null && user.Claims != null && user.Claims.Any())
            {
                var identification = user.Claims.FirstOrDefault(c => c.Type.Equals(nameof(Identification)));

                Identification = identification != null ? identification.Value : "";
            }
        }
    }
}