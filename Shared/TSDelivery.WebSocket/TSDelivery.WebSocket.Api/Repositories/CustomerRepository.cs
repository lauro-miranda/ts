using LM.Responses;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Threading.Tasks;
using TSDelivery.WebSocket.Api.Domain.Models;
using TSDelivery.WebSocket.Api.Domain.Settings;
using TSDelivery.WebSocket.Api.Repositories.Contracts;

namespace TSDelivery.WebSocket.Api.Repositories
{
    public class DeliverymanRepository : IDeliverymanRepository
    {
        protected IMongoCollection<Deliveryman> Collection { get; }

        public DeliverymanRepository(IOptions<NoSQLSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);

            Collection = database.GetCollection<Deliveryman>(settings.Value.DeliverymanCollectionName);
        }

        public async Task CreateOrUpdateAsync(Deliveryman deliveryman)
        {
            await DeleteAsync(deliveryman);
            await Collection.InsertOneAsync(deliveryman);
        }

        public async Task<Maybe<Deliveryman>> FindAsync(string identification)
            => await (await Collection
                .FindAsync(a => a.Identification.Equals(identification)))
                .FirstOrDefaultAsync();

        public async Task DeleteAsync(string identification)
            => await Collection.DeleteOneAsync(x => x.Identification.Equals(identification));

        async Task DeleteAsync(Deliveryman deliveryman)
            => await DeleteAsync(deliveryman.Identification);
    }
}