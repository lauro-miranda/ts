using LM.Domain.Entities;
using LM.Responses;
using LM.Responses.Extensions;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Text;

namespace TSDelivery.Accounts.Api.Domain
{
    public class User : Entity
    {
        [Obsolete(ConstructorObsoleteMessage, true)]
        User() { }
        public User(string name
            , string identification
            , string password)
            : base(Guid.NewGuid())
        {
            Name = name;
            Identification = identification;
            Password = password;
        }

        public string Name { get; private set; }

        public string Identification { get; private set; }

        public string Password { get; private set; }

        public Response Login(string password
            , string encryptionKey)
        {
            var response = Response.Create();

            if (!Password.Equals(EncriptyPassword(password, encryptionKey)))
                response.WithBusinessError(nameof(password), "Usuário e/ou senha inválido(s).");

            if (response.HasError)
                return response;

            return response;
        }

        public static Response<User> Create(string name
            , string identification
            , string password
            , string encryptionKey)
        {
            var response = Response<User>.Create();

            if (string.IsNullOrEmpty(name))
                response.WithBusinessError(nameof(name), "O nome não foi informado.");

            if (string.IsNullOrEmpty(identification))
                response.WithBusinessError(nameof(identification), "A identificação não foi informada.");

            if (string.IsNullOrEmpty(password))
                response.WithBusinessError(nameof(password), "A senha não foi informada.");

            if (response.HasError)
                return response;

            var user = new User(name, identification, EncriptyPassword(password, encryptionKey));

            return response.SetValue(user);
        }

        static string EncriptyPassword(string password, string encryptionKey)
        {
            var valueBytes = KeyDerivation.Pbkdf2(
                                password: password,
                                salt: Encoding.UTF8.GetBytes(encryptionKey),
                                prf: KeyDerivationPrf.HMACSHA512,
                                iterationCount: 10000,
                                numBytesRequested: 256 / 8);

            return Convert.ToBase64String(valueBytes);
        }

        public static implicit operator User(Response<User> response) => response.Data.Value;

        public static implicit operator User(Maybe<User> maybe) => maybe.Value;
    }
}