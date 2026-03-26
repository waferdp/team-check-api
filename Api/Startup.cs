using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Repository.Interface;
using Repository;
using DomainModel;
using Microsoft.FeatureManagement;
using Microsoft.Extensions.Configuration;
using Repository.Configuration;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson;

namespace Api
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        private static readonly string CorsPolicyAllowEverything = "allow_everything";
        
        public static void ConfigureBuilder(WebApplicationBuilder builder)
        {
            var connectionStringsConfig = builder.Configuration.GetSection("ConnectionStrings").Get<ConnectionStringsConfig>();
            builder.Services.AddSingleton(connectionStringsConfig);

            if(!string.IsNullOrEmpty(connectionStringsConfig.AppConfig))
            {
                builder.Configuration.AddAzureAppConfiguration(options =>
                    options.Connect(connectionStringsConfig.AppConfig).UseFeatureFlags());
            }

            ConfigureServices(builder.Services);
        }
        
        private static void ConfigureServices(IServiceCollection services)
        {
            
            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicyAllowEverything,
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader();
                    });
            });

            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
            services.AddControllers();
            services.AddSwaggerGen();
            services.AddAzureAppConfiguration();
            services.AddFeatureManagement();
            InjectRepositories(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseAzureAppConfiguration();
            }
            app.UseCors(CorsPolicyAllowEverything);
            app.UseRouting();
            app.UseSwagger();
            app.UseSwaggerUI(options => 
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "TeamCheck API v1");
                options.RoutePrefix = string.Empty;
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void InjectRepositories(IServiceCollection services)
        {
            services.AddScoped<IRepository<Team>, TeamRepository>();
            services.AddScoped<IRepository<TeamAnswer>, SimpleRepository<TeamAnswer>>();
        }
    }
}
