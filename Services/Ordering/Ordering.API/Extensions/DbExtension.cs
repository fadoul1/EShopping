﻿using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Ordering.API.Extensions;

public static class DbExtension
{
    public static IHost MigrateDatabase<TContext>(
        this IHost host,
        Action<TContext, IServiceProvider> seeder
    )
        where TContext : DbContext
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<TContext>>();
            var context = services.GetService<TContext>();

            try
            {
                logger.LogInformation("Started Db Migration: {context}", typeof(TContext).Name);
                CallSeeder(seeder!, context!, services!);
                logger.LogInformation("Migration Completed: {context}", typeof(TContext).Name);
            }
            catch (SqlException e)
            {
                logger.LogError(
                    e,
                    "An error occurred while migrating db: {context}",
                    typeof(TContext).Name
                );
            }
        }

        return host;
    }

    private static void CallSeeder<TContext>(
        Action<TContext, IServiceProvider> seeder,
        TContext context,
        IServiceProvider services
    )
        where TContext : DbContext
    {
        context.Database.Migrate();
        seeder(context, services);
    }
}
