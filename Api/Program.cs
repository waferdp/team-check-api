using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration(config =>
                    {
                        var settings = config.Build();
                        var appConfigConnectionString = settings["ConnectionStrings:AppConfig"];
                        if(!string.IsNullOrEmpty(appConfigConnectionString))
                        {
                            config.AddAzureAppConfiguration(options => 
                                options.Connect(appConfigConnectionString).UseFeatureFlags());
                        }
                        
                    }).UseStartup<Startup>();
                });

    }
}
