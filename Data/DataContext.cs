using System;
using System.IO;
using System.Threading.Tasks;
using Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
    public class DataContext : DbContext
    {
        public DbSet<AppUser> Users { get; set; }

        public DataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var allEntities = modelBuilder.Model.GetEntityTypes();

            foreach(var entity in allEntities)
            {
                entity.AddProperty("CreateDate", typeof(DateTime));
                entity.AddProperty("ModifyDate", typeof(DateTime));
            }
        }

        private readonly StreamWriter _logStream = new StreamWriter("AppLog/ContextLog.txt", append: true);

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.LogTo(_logStream.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);

        public override void Dispose()
        {
            base.Dispose();
            _logStream.Dispose();
        }

        public override async ValueTask DisposeAsync()
        {
            await base.DisposeAsync();
            await _logStream.DisposeAsync();
        }

    }
}