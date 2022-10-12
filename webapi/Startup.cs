using System.Data;
using Grace.AspNetCore.MVC;
using Grace.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebApi.Helpers;
using WebApi.Models;

namespace WebApi
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options => options.Conventions.Add(new RouteTokenTransformerConvention(new KebabCaseParameterTransformer())))
            .AddNewtonsoftJson(options => options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc)
            .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddSwaggerGen();

            services.AddLogging(logging => logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning));
            services.AddCors(options => options.AddPolicy(name: "AllowLocalhost", policy => policy
            .WithOrigins("http://localhost:3000")
            .WithMethods("GET", "POST", "PUT", "DELETE")
            .WithHeaders("Content-Type")
            ));

            var connectionString = Configuration.GetConnectionString("WebApiDatabase");
            services.AddTransient<IDbConnection>(_ =>
                {
                    var connection = SqlClientFactory.Instance.CreateConnection();
                    if (connection == null) return null!;
                    connection.ConnectionString = connectionString;
                    return connection;

                });

            services.AddDbContextPool<DataContext>(options => options
                .UseSqlServer(connectionString));
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("Logs/log-{Date}.log");
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("AllowLocalhost");
            app.UseEndpoints(endpoints => endpoints.MapControllers());
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        public void ConfigureContainer(IInjectionScope scope)
        {
            scope.SetupMvc();
        }
    }
}
