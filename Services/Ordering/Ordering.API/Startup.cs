﻿using EventBus.Messages.Common;
using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Ordering.API.EventBusConsumer;
using Ordering.Application.Extensions;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Extensions;

namespace Ordering.API;

public class Startup(IConfiguration configuration)
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddApiVersioning();
        services.AddApplicationServices();
        services.AddInfraServices(configuration);
        services.AddAutoMapper(typeof(Startup));

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ordering.API", Version = "v1" });
        });
        services.AddHealthChecks().Services.AddDbContext<OrderContext>();

        services.AddMassTransit(config =>
        {
            config.AddConsumer<BasketOrderingConsumer>();
            config.UsingRabbitMq(
                (ct, cfg) =>
                {
                    cfg.Host(configuration["EventBusSettings:HostAddress"]);
                    //provide the queue name with consumer settings
                    cfg.ReceiveEndpoint(
                        EventBusConstants.BasketCheckoutQueue,
                        c =>
                        {
                            c.ConfigureConsumer<BasketOrderingConsumer>(ct);
                        }
                    );
                }
            );
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ordering.API v1"));
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks(
                "/health",
                new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                }
            );
        });
    }
}
