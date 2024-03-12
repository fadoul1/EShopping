using System.Diagnostics;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace Basket.API;

public class Program
{
    public static void Main(string[] args)
    {
        Activity.DefaultIdFormat = ActivityIdFormat.W3C;
        CreateHostBuilder(args).Build().Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
