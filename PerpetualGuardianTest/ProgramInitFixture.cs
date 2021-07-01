using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PerpetualGuardian.Data;
using PerpetualGuardian.Services;
using PerpetualGuardian.Services.Implement;
using System;
using System.IO;
using Xunit;

namespace PerpetualGuardianTest
{
    public class ProgramInitFixture : IDisposable
    {
        public IServiceCollection Services { get; }
        public IServiceProvider Provider { get; }

        public ProgramInitFixture()
        {
            Console.WriteLine("Build all services!");

            Services = new ServiceCollection();

            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile( "appsettings.json", optional: false, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();

            Services.AddSingleton<IConfiguration>(_ => configuration);

            Services.AddDbContext<MyDbContext>(
                option => { option.UseSqlite("Data Source=routine.db"); }
            );

            Services.AddScoped<IFileInformationService, FileInformationService>();

            Provider = Services.BuildServiceProvider();
            Console.WriteLine("All services have been built!");
            this.Init();
        }

        private void Init()
        {
            Console.WriteLine("Init the database!");
            var dbContext = Provider.GetService<MyDbContext>();
            dbContext.Database.EnsureDeleted();
            dbContext.Database.Migrate();
            dbContext.SaveChanges();
            Console.WriteLine("The database has initilized!");
        }

        public void Dispose()
        {
            Console.WriteLine("Clean all services and the database!");
            var dbContext = Provider.GetService<MyDbContext>();
            dbContext.Database.EnsureDeleted();

            Services.Clear();
            Console.WriteLine("All services and the database have been cleaned!");
        }
    }

    [CollectionDefinition("ProgramInitCollection")]
    public class ProgramInitCollection : ICollectionFixture<ProgramInitFixture>
    {

    }
}
