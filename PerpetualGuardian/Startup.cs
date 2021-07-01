using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PerpetualGuardian.Data;
using PerpetualGuardian.Services;
using PerpetualGuardian.Services.Implement;

namespace PerpetualGuardian
{
    public class Startup
    {
        private const string CORS_POLICY_NAME = "MyPolicy";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            #region CORS
            services.AddCors(c =>
            {
                c.AddPolicy(CORS_POLICY_NAME, policy =>
                {
                    policy
                    .WithOrigins("http://127.0.0.1:3000", "http://localhost:3000")  //allow client ip
                    .WithExposedHeaders("x-pagination", "location")                  //allow header
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
            #endregion

            services.AddDbContext<MyDbContext>(option =>
            {
                option.UseSqlite("Data Source=routine.db");
            });

            services.AddScoped<IFileInformationService, FileInformationService>();  //add file information service
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region CORS
            app.UseCors(CORS_POLICY_NAME);
            #endregion

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
