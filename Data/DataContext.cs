using System;
using System.IO;
using System.Threading.Tasks;
using Api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Api.Data
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int,
            IdentityUserClaim<int>,AppUserRole, IdentityUserLogin<int>,
            IdentityRoleClaim<int>, IdentityUserToken<int>>
    {

        private static ILogger _logger;

        public DataContext(DbContextOptions options, ILogger logger) : base(options)
        {
            _logger = logger;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
                    .HasMany(ur => ur.UserRole)
                    .WithOne(u => u.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();

            builder.Entity<AppRole>()
                    .HasMany(ur => ur.UserRoles)
                    .WithOne(u => u.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
            //var allEntities = builder.Model.GetEntityTypes();

            //foreach(var entity in allEntities)
            //{
            //    entity.AddProperty("CreateDate", typeof(DateTime));
            //    entity.AddProperty("ModifyDate", typeof(DateTime));
            //}
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.LogTo(_logger.Information);



    }
}