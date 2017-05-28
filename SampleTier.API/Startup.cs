
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SampleTier.API.Models;
using Microsoft.EntityFrameworkCore;
using SampleTier.DataAccess.Uow;


namespace SampleTier.API
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connection = @"Server=(localdb)\mssqllocaldb;Database=testdb;Trusted_Connection=True;";
            services.AddDbContext<AngularContext>(options => options.UseSqlServer(connection));
            services.AddScoped<IUnitOfWorkBase, UnitOfWorkBase>();

            services.AddCors(options => options.AddPolicy("AllowWebApp",
              builder => builder.AllowAnyMethod()
                         .AllowAnyMethod()
                         .AllowAnyOrigin()
                         .AllowAnyHeader()));

            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            // Shows UseCors with CorsPolicyBuilder.
            app.UseCors("AllowWebApp");
            app.UseMvc();
        }
    }
}
