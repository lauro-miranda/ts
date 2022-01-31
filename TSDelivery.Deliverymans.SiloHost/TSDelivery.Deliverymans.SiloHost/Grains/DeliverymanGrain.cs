using LM.Domain.Valuables;
using LM.Responses;
using LM.Responses.Extensions;
using Orleans;
using Orleans.Providers;
using Orleans.Runtime;
using System;
using System.Threading.Tasks;

namespace TSDelivery.Deliverymans.SiloHost.Grains
{
    public interface IDeliverymanGrain : IGrainWithGuidKey
    {
        Task<Response<Deliveryman>> Get();

        Task<Response> Create(DeliverymanRequest request);
    }

    public class DeliverymanRequest
    {
        public Guid Code { get; set; }

        public string Name { get; set; }

        public string CPF { get; set; }
    }

    [StorageProvider(ProviderName = "deliverymans")]
    public class DeliverymanGrain : Grain, IDeliverymanGrain
    {
        IPersistentState<Deliveryman> DeliverymanState { get; }

        public DeliverymanGrain([PersistentState("deliverymans", storageName: "deliverymans")] IPersistentState<Deliveryman> state)
        {
            DeliverymanState = state;
        }

        public async Task<Response<Deliveryman>> Get()
        {
            return await Task.FromResult(DeliverymanState.State);
        }

        public async Task<Response> Create(DeliverymanRequest request)
        {
            var response = Deliveryman.Create(request);

            if (response.HasError)
                return response;

            DeliverymanState.State = response.Data.Value;
            await DeliverymanState.WriteStateAsync();

            return response;
        }
    }

    public class Deliveryman
    {
        public Deliveryman() { }
        Deliveryman(Guid code
            , string name
            , CPF cpf)
        {
            Code = code;
            Name = name;
            CPF = cpf;
            Active = true;
        }

        public Guid Code { get; private set; }

        public string Name { get; private set; }

        public CPF CPF { get; private set; }

        public bool Active { get; private set; }

        public static Response<Deliveryman> Create(DeliverymanRequest request)
        {
            var response = Response<Deliveryman>.Create();

            if (string.IsNullOrEmpty(request.Name))
                response.WithBusinessError(nameof(request.Name), "O Nome do entregador não foi informado.");

            if (request.CPF == null)
                response.WithBusinessError(nameof(request.CPF), "O CPF do entregador não foi informado.");

            if (response.HasError)
                return response;

            return response.SetValue(new Deliveryman(request.Code
                , request.Name
                , CPF.Create(request.CPF)));
        }
    }
}