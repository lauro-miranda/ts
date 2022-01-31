using EasyNetQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MassTransit;

namespace RabbitMQ.Issuers.Api
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

            services.AddMassTransit(cfg =>
            {
                cfg.UsingRabbitMq((context, config) =>
                {
                    config.Host(Configuration.GetSection(nameof(RabbitMQSettings)).GetValue<string>(nameof(RabbitMQSettings.BaseURL)), "/", h =>
                    {
                        h.Username(Configuration.GetSection(nameof(RabbitMQSettings)).GetValue<string>(nameof(RabbitMQSettings.UserName)));
                        h.Password(Configuration.GetSection(nameof(RabbitMQSettings)).GetValue<string>(nameof(RabbitMQSettings.Password)));
                    });
                });
            });

            services.AddMassTransitHostedService();

            services.AddSingleton(RabbitHutch.CreateBus(Configuration.GetSection("QueueSettings:Connection").Value).Advanced);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}