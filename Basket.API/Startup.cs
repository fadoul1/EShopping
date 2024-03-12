using System.Reflection;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Basket.Application.GrpcService;
using Basket.Application.Handlers;
using Basket.Core.Repositories;
using Basket.Infrastructure.Repositories;
using Discount.Grpc.Protos;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Basket.API;

public class Startup
{
    public IConfiguration Configuration;

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
        });

        services.AddCors(options =>
        {
            options.AddPolicy(
                "CorsPolicy",
                policy =>
                {
                    //TODO read the same from settings for prod deployment
                    policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                }
            );
        });

        //Redis Settings
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = Configuration.GetValue<string>(
                "CacheSettings:ConnectionString"
            );
        });

        services.AddAutoMapper(typeof(Startup));
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblies(
                typeof(CreateShoppingCartCommandHandler).GetTypeInfo().Assembly
            )
        );

        services.AddScoped<IBasketRepository, BasketRepository>();

        services.AddScoped<DiscountGrpcService>();
        services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(o =>
            o.Address = new Uri(Configuration["GrpcSettings:DiscountUrl"])
        );

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(
                "v1",
                new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Basket.API", Version = "v1" }
            );
        });

        services
            .AddHealthChecks()
            .AddRedis(
                Configuration["CacheSettings:ConnectionString"],
                "Redis Health",
                HealthStatus.Degraded
            );
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket.API v1")
            );
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors("CorsPolicy");
        app.UseAuthentication();
        app.UseStaticFiles();
        app.UseAuthorization();
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
