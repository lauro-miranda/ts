using LM.Responses;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Threading.Tasks;
using TSDelivery.Accounts.Api.Domain;
using TSDelivery.Accounts.Api.Domain.Repositories.Contracts;

namespace TSDelivery.Accounts.Api.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        protected IMongoCollection<User> Collection { get; }

        public UserRepository(IOptions<NoSQLSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);

            Collection = database.GetCollection<User>(settings.Value.UserCollectionName);
        }

        public async Task CreateOrUpdateAsync(User user)
        {
            await DeleteAsync(user);
            await Collection.InsertOneAsync(user);
        }

        public async Task<Maybe<User>> FindAsync(string identification)
            => await (await Collection
                .FindAsync(a => a.Identification.Equals(identification)))
                .FirstOrDefaultAsync();

        public async Task DeleteAsync(User user)
            => await Collection.DeleteOneAsync(x => x.Identification.Equals(user.Identification));
    }
}
