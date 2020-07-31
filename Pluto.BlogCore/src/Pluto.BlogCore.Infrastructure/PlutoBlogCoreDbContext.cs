using MediatR;

using Microsoft.EntityFrameworkCore;
using Pluto.BlogCore.Infrastructure.EntityTypeConfigurations;
using Pluto.BlogCore.Domain.DomainModels.Account;

namespace Pluto.BlogCore.Infrastructure
{
    public class PlutoBlogCoreDbContext : DbContext
    {

        public PlutoBlogCoreDbContext(DbContextOptions<PlutoBlogCoreDbContext> options)
            : base(options)
        {
        }


        #region Entitys and configuration  (OnModelCreating中配置了对应Entity 那么对应DbSet<>可以不写)
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<UserRoleEntity> UserRoles { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfig());
            modelBuilder.ApplyConfiguration(new RoleEntityTypeConfig());
            modelBuilder.ApplyConfiguration(new UserRoleEntityTypeConfig());
        }
        #endregion

    }
}
