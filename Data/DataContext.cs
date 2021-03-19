using System;
using System.IO;
using System.Threading.Tasks;
using Api.Entities;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Api.Data
{
    public class DataContext : DbContext
    {
        public DbSet<AppUser> Users { get; set; }
        private static ILogger _logger;

        public DataContext(DbContextOptions options, ILogger logger) : base(options)
        {
            _logger = logger;
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

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //=> optionsBuilder.LogTo(_logger.Information);

    }
}