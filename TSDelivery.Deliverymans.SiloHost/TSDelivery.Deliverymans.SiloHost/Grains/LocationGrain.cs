using LM.Responses;
using LM.Responses.Extensions;
using Orleans;
using Orleans.Providers;
using Orleans.Runtime;
using System;
using System.Threading.Tasks;
using TSDelivery.Deliverymans.SiloHost.Grains.Models;

namespace TSDelivery.Deliverymans.SiloHost.Grains
{
    public interface ILocationGrain : IGrainWithStringKey
    {
        Task<Response<Location>> Get();

        Task<Response> Set(Location location);
    }

    [StorageProvider(ProviderName = "locations")]
    public class LocationGrain : Grain, ILocationGrain
    {
        IPersistentState<Location> Location { get; set; }

        public LocationGrain([PersistentState("locations", storageName: "locations")] IPersistentState<Location> location)
        {
            Location = location;
        }

        public override Task OnActivateAsync()
        {
            return Task.CompletedTask;
        }

        public async Task<Response<Location>> Get()
        {
            return await Task.FromResult(Location.State);
        }

        public async Task<Response> Set(Location location)
        {
            var response = Response.Create();

            if (location == null)
                return response.WithBusinessError(nameof(location), "Localização não informada.");

            if (location.Latitude == 0)
                response.WithBusinessError(nameof(location.Latitude), $"A '{nameof(location.Latitude)}' é inválida.");

            if (location.Latitude == 0)
                response.WithBusinessError(nameof(location.Latitude), $"A '{nameof(location.Latitude)}' é inválida.");

            if (response.HasError)
                return response;

            Location.State = location;
            Location.State.UpdatedAt = DateTime.Now;

            await Location.WriteStateAsync();

            return response;
        }
    }
}