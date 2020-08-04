using MediatR;

using Microsoft.EntityFrameworkCore;
using Pluto.BlogCore.Domain.DomainModels.Blog;
using Pluto.BlogCore.Infrastructure.EntityTypeConfigurations;


namespace Pluto.BlogCore.Infrastructure
{
    public class PlutoBlogCoreDbContext : DbContext
    {

        public PlutoBlogCoreDbContext(DbContextOptions<PlutoBlogCoreDbContext> options)
            : base(options)
        {
        }


        #region Entitys and configuration  (OnModelCreating中配置了对应Entity 那么对应DbSet<>可以不写)

        public DbSet<Post> Posts { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PostConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new TagConfiguration());
            modelBuilder.ApplyConfiguration(new PostTagConfiguration());
        }
        #endregion

    }
}
