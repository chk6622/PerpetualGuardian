using Microsoft.EntityFrameworkCore;
using PerpetualGuardian.Entities;

namespace PerpetualGuardian.Data
{
    public class MyDbContext:DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }

        public DbSet<FileInformation> FileInformationSet { get; set; }
    }
}
