using System.Linq;
using ApplicationWireUp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApplication.Middlewares;

namespace WebApplication
{
    public class Startup
    {
        private ApplicationContainer _applicationContainer;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _applicationContainer = ApplicationContainer.Build(
                Configuration.EventStoreConnectionString(),
                Configuration.EventStoreName(),
                Configuration.AggregateTypeCacheExpirations());
            
            services.AddSingleton(_applicationContainer);
            
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                                  builder =>
                                  {
                                      builder.AllowAnyHeader()
                                          .AllowAnyMethod()
                                          .WithOrigins(Configuration.AllowedCorsOrigins().ToArray());
                                  });
            });
            
            services.AddControllers().AddNewtonsoftJson();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseMiddleware<BadRequestExceptionMiddleware>();
            app.UseRouting();
            app.UseCors("AllowSpecificOrigin");
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}