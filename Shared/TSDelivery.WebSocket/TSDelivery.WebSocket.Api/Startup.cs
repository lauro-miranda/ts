using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TSDelivery.WebSocket.Api.Domain;
using TSDelivery.WebSocket.Api.Domain.Settings;
using TSDelivery.WebSocket.Api.Extensions;
using TSDelivery.WebSocket.Api.Hubs;
using TSDelivery.WebSocket.Api.Repositories;
using TSDelivery.WebSocket.Api.Repositories.Contracts;

namespace TSDelivery.WebSocket.Api
{
    public class Startup
    {
        IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddScoped<IUser, User>()
                .AddTransient<IDeliverymanRepository, DeliverymanRepository>();
            services.Configure<NoSQLSettings>(Configuration.GetSection(nameof(NoSQLSettings)));

            services.AddJWTAuthentication(Configuration.GetSection("JTWSettings:Secret").Value);

            services.AddHttpContextAccessor();

            services.AddCors();

            services
                .AddSignalR()
                .AddRedis(Configuration.GetSection("Redis:Connection").Value);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials());

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapHub<DeliverymanHub>($"/{nameof(DeliverymanHub)}");
            });
        }
    }
}