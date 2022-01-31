using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace SignalR.Cliente.App
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZGVudGlmaWNhdGlvbiI6IjA1OTI0MjgyNzMyIiwibmJmIjoxNjIxNjIzOTczLCJleHAiOjE2MjE2Mjc1NzMsImlhdCI6MTYyMTYyMzk3M30.QqlSOlon2cw9nN1b5o6SEyhZZe5ymnTdB9LrSbR2Uu0";

            var connection = new HubConnectionBuilder()
                .WithUrl("http://127.0.0.1:49518/ShopHub", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(token);
                })
                .WithAutomaticReconnect()
                .Build();

            connection.On<string>("OnMessage", (message) =>
            {
                Console.WriteLine(message);
            });

            while (true)
            {
                try
                {
                    await connection.StartAsync();

                    break;
                }
                catch (Exception ex)
                {
                    await Task.Delay(1000);
                }
            }

            Console.WriteLine("Aguardando mensagens.");
            Console.ReadLine();
        }

        public class NewDeliveryDto
        {
            public string Number { get; set; }

            public string Name { get; set; }

            public string Address { get; set; }
        }
    }
}