using System.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Grace.AspNetCore.MVC;
using Grace.DependencyInjection;
using WebApi.Models;

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
            services.AddControllers();

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

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        public void ConfigureContainer(IInjectionScope scope)
        {
            scope.SetupMvc();
        }
    }
}
