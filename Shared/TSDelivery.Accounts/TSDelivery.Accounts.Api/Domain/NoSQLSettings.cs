namespace TSDelivery.Accounts.Api.Domain
{
    public class NoSQLSettings
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }

        public string UserCollectionName { get; set; }
    }
}