using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace SignalR.Cliente.App._02
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // staging
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZGVudGlmaWNhdGlvbiI6IjA1OTI0MjgyNzMyIiwibmJmIjoxNjQyMjc1NDE5LCJleHAiOjE2NDIyNzkwMTksImlhdCI6MTY0MjI3NTQxOX0.dN_vnbm8QKXhRj99Pq37HL_ze_dERGNnHn1SrL_NscQ";

            var connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:44310/DeliverymanHub", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZGVudGlmaWNhdGlvbiI6IjA1OTI0MjgyNzMyIiwibmJmIjoxNjQyMjgyNzQ5LCJleHAiOjE2NDIyODYzNDksImlhdCI6MTY0MjI4Mjc0OX0.UvUxetqmDn_dNsgiwQcrDlAQRJqPKXvH4yCXIMygFSg");
                })
                .WithAutomaticReconnect()
                .Build();

            await connection.StartAsync();

            connection.On<string>("OnMessage", (a) =>
            {
                Console.WriteLine(a);
            });
            
            connection.On<object>("OnOldDelivery", (a) =>
            {


                connection.InvokeAsync("OnLocalization", new LocationModel
                {
                    Latitude = -22.97566250,
                    Longitude = -43.18967930
                });
            });

            Console.WriteLine("Aguardando novas entregas.");
            Console.ReadLine();            
        }

        public class LocationModel
        {
            public double Latitude { get; set; }

            public double Longitude { get; set; }
        }

        public class NewDeliveryDto
        {
            public string Number { get; set; }

            public string Name { get; set; }

            public string Address { get; set; }
        }
    }
}