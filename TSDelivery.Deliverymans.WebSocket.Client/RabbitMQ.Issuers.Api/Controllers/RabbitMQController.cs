using DeliveryApp.Messages.Enums;
using DeliveryApp.Messages.Orders;
using EasyNetQ;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RabbitMQ.Issuers.Api.Controllers
{
    [ApiController, Route("")]
    public class RabbitMQController : Controller
    {
        IBusControl BusControl { get; }

        public RabbitMQController(IAdvancedBus bus
            , IBusControl busControl)
            : base(bus)
        {
            BusControl = busControl;
        }

        [HttpGet, Route("")]
        public IActionResult Get() => Ok(new { });

        [HttpPost, Route("send")]
        public async Task<IActionResult> SendAsync()
        {
            await RiseAsync(new OrderCreatedMessage
            {
                Id = 1,
                DeliveryFee = 0,
                PaymentType = PaymentType.pre_paid,
                SubTotal = 12.00M,
                Total = 12.00M,
                TotalItens = 1,
                Items = new List<OrderItem>
                {
                    new OrderItem
                    {
                        Product =new Product
                        {
                            Brand = "teste",
                            Dosage = "12",
                            Ean = "123123",
                            FormQuantity = "123",
                            Name = "Aspirina"
                        },
                        DiscountPrice = 12.00M,
                        Price = 12.00M,
                        Quantity = 1,
                        TotalDiscountPrice = 0,
                        TotalPrice = 12.00M
                    }
                },
                Drugstore = new DrugstoreOrderCreated
                {
                    Id = 121224,
                    StoreId = "121224",
                    Cnpj = "19827940000187",
                    GroupCnpj = "19.827.940/0001-87",
                    GroupCompanyName = "Drogaria Lider de Nova Iguacu LTDA | Rede Economia",
                    GroupLogo = "",
                    GroupTradeName = "",
                    Name = "Rede Economia",
                    Phone = "(21) 2796-6279",
                    PhoneTwo = "(21) 2796-6279",
                    Address = new Address
                    {
                        City = "Rio de Janeiro",
                        Complement = "",
                        Latitude = -22.9682661M,
                        Longitude = -43.183698199999988M,
                        Neighborhood = "Recreio dos Bandeirantes",
                        Number = "158",
                        State = "RJ",
                        Street = "R. Gen. Landri Gonçalves",
                        ZipCode = "22795-410"
                    }
                },
                User = new UserOrderCreated
                {
                    Id = 1,
                    Name = "Lauro Miranda",
                    Cpf = "05924282732",
                    Gender = '1',
                    PhoneNumber = "(21) 9 7004-2060",
                    DeliveryAddress = new Address
                    {
                        City = "Rio de Janeiro",
                        Complement = "",
                        Latitude = -23.0285496M,
                        Longitude = -43.4787083M,
                        Neighborhood = "Recreio dos Bandeirantes",
                        Number = "252",
                        State = "RJ",
                        Street = "Rua D-W",
                        ZipCode = "22795-780"
                    }
                }
            });

            return Ok();
        }

        [HttpPost, Route("send-balance")]
        public async Task<IActionResult> RiseBalanceAsync()
        {
            await RiseBalanceAsync(new OrderCreatedMessage
            {
                Id = 121224,
                DeliveryFee = 0,
                PaymentType = PaymentType.pre_paid,
                SubTotal = 12.00M,
                Total = 12.00M,
                TotalItens = 1,
                Items = new List<OrderItem>
                {
                    new OrderItem
                    {
                        Product =new Product
                        {
                            Brand = "teste",
                            Dosage = "12",
                            Ean = "123123",
                            FormQuantity = "123",
                            Name = "Aspirina"
                        },
                        DiscountPrice = 12.00M,
                        Price = 12.00M,
                        Quantity = 1,
                        TotalDiscountPrice = 0,
                        TotalPrice = 12.00M
                    }
                },
                Drugstore = new DrugstoreOrderCreated
                {
                    Id = 1,
                    StoreId = "121224",
                    Cnpj = "19827940000187",
                    GroupCnpj = "19.827.940/0001-87",
                    GroupCompanyName = "Saldo Liberado",
                    GroupLogo = "",
                    GroupTradeName = "",
                    Name = "Rede Economia",
                    Phone = "(21) 2796-6279",
                    PhoneTwo = "(21) 2796-6279",
                    Address = new Address
                    {
                        City = "Rio de Janeiro",
                        Complement = "",
                        Latitude = -22.9682661M,
                        Longitude = -43.183698199999988M,
                        Neighborhood = "Recreio dos Bandeirantes",
                        Number = "158",
                        State = "RJ",
                        Street = "R. Gen. Landri Gonçalves",
                        ZipCode = "22795-410"
                    }
                },
                User = new UserOrderCreated
                {
                    Id = 1,
                    Name = "Lauro Miranda",
                    Cpf = "05924282732",
                    Gender = '1',
                    PhoneNumber = "(21) 9 7004-2060",
                    DeliveryAddress = new Address
                    {
                        City = "Rio de Janeiro",
                        Complement = "",
                        Latitude = -23.0285496M,
                        Longitude = -43.4787083M,
                        Neighborhood = "Recreio dos Bandeirantes",
                        Number = "252",
                        State = "RJ",
                        Street = "Rua D-W",
                        ZipCode = "22795-780"
                    }
                }
            });

            return Ok();
        }

        public async Task RiseBalanceAsync(OrderCreatedMessage message)
        {
            var sender = await BusControl.GetSendEndpoint(new Uri(string.Format("queue:{0}", "BackofficeAPI.balanceRelease.staging")));

            await sender.Send(message);
        }

        public async Task RiseAsync(OrderCreatedMessage message)
        {
            var sender = await BusControl.GetSendEndpoint(new Uri(string.Format("queue:{0}", "BackofficeAPI.orderDelivery.staging")));

            await sender.Send(message);
        }
    }
}