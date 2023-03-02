using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Store.Models;
using Store.Services;

namespace Store
{
    public class MyStoreContext : IdentityDbContext<UserModel>
    {
        public DbSet<AddressModel> Address { get; set; }
        public DbSet<ProductModel> Product { get; set; }
        public DbSet<ByProductDetailModel> ByProductDetail { get; set; }
        public DbSet<CartModel> Cart { get; set; }
        public DbSet<DetailCartModel> DetailCart { get; set; }

        public MyStoreContext(DbContextOptions<MyStoreContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLoggerFactory(LoggerService.Factory);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                string tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }

        }
    }
}
