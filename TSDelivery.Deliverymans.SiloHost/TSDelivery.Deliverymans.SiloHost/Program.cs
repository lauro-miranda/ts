using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Hosting;
using TSDelivery.Deliverymans.SiloHost.Grains;

namespace TSDelivery.Deliverymans.SiloHost
{
    public class Program
    {
        static string RedisConnection { get; } = "127.0.1.1:6379,password=12345678";

        public static void Main(string[] args)
        {
            Host.CreateDefaultBuilder(args)
             .UseOrleans((ctx, builder) =>
             {
                 if (ctx.HostingEnvironment.IsDevelopment())
                 {
                     builder.UseLocalhostClustering();
                     builder.AddMemoryGrainStorage("locations");
                     builder.AddMemoryGrainStorage("deliverymans");
                 }
                 else
                 {
                     builder.UseKubernetesHosting();

                     builder.UseRedisClustering(options => options.ConnectionString = RedisConnection);
                     builder.AddRedisGrainStorage("locations", options => options.ConnectionString = RedisConnection);
                     builder.AddRedisGrainStorage("deliverymans", options => options.ConnectionString = RedisConnection);
                 }

                 builder.UseDashboard(options =>
                 {
                     options.Port = 8888;
                 })
                 .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(LocationGrain).Assembly))
                 .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(Deliveryman).Assembly));
             })
             .ConfigureWebHostDefaults(webBuilder =>
             {
                 webBuilder.UseStartup<Startup>();
             })
             .RunConsoleAsync()
             .GetAwaiter()
             .GetResult();
        }
    }
}