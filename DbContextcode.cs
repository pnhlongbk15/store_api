using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Store.Models;
using System.Reflection;

namespace Store
{

    public class MyStoreContext1 : IdentityDbContext<UserModel>
    {
        public DbSet<AddressModel> Address { get; set; }
        public DbSet<ProductModel> Product { get; set; }
        public DbSet<ByProductDetailModel> ByProductDetail { get; set; }
        public DbSet<CartModel> Cart { get; set; }
        public DbSet<DetailCartModel> DetailCart { get; set; }

        private readonly IConfiguration _configuration;
        private bool _databaseChecked = false;

        public MyStoreContext1(DbContextOptions<MyStoreContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //base.OnConfiguring(optionsBuilder);
            // optionsBuilder.UseLoggerFactory(LoggerService.Factory);
            Console.WriteLine("OnConfiguring");
            optionsBuilder.UseMySQL(_configuration.GetConnectionString("MyStore"));

            if (!_databaseChecked)
            {
                CheckDatabaseConnection();
                _databaseChecked = true;
            }
        }

        private void CheckDatabaseConnection()
        {
            try
            {
                Database.GetDbConnection();
            }
            catch (MySqlException ex)
            {
                var optionsBuilder = new DbContextOptionsBuilder<MyStoreContext>();
                optionsBuilder.UseMySQL(_configuration.GetConnectionString("Backup"));
                using (var backupContext = new MyStoreContext1(optionsBuilder.Options, _configuration))
                {
                    // Make sure the alternative database exists and is up-to-date
                    backupContext.Database.EnsureCreated();
                    backupContext.Database.Migrate();

                    //Database.GetDbConnection().Open();
                    backupContext.Database.OpenConnection();

                    // Use reflection to set the private _database field of the current context
                    var databaseField = typeof(DbContext).GetField("_database", BindingFlags.NonPublic | BindingFlags.Instance);
                    databaseField.SetValue(this, backupContext.Database);
                }
            }
        }
    }
}
