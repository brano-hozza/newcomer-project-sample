using System.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog.Extensions.Logging;
using Grace.AspNetCore.MVC;
using Grace.DependencyInjection;
using WebApi.Models;
using AutoMapper;

namespace WebApi.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostEnvironment HostingEnvironment { get; }

        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            Configuration = configuration;
            HostingEnvironment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options => options.Conventions.Add(new RouteTokenTransformerConvention(new KebabCaseParameterTransformer())))
            .AddNewtonsoftJson(options => options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc)
            .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
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
        }

        public void ConfigureContainer(IInjectionScope scope)
        {
            scope.SetupMvc();
        }
    }
}
