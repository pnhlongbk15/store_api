using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Store.Middlewares
{
    public class DatabaseCheckMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public DatabaseCheckMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context, MyStoreContext dbContext)
        {
            if (!dbContext.Database.CanConnect())
            {
                var backupConnectionString = _configuration.GetConnectionString("Backup");
                var optionsBuilder = new DbContextOptionsBuilder<MyStoreContext>();
                optionsBuilder.UseMySQL(backupConnectionString);
                using var backupDbContext = new MyStoreContext(optionsBuilder.Options);

                if (!backupDbContext.Database.CanConnect())
                {
                    // Both databases are unavailable, return an error response
                    context.Response.StatusCode = 503; // Service Unavailable
                    await context.Response.WriteAsync("Both databases are currently unavailable.");
                    return;
                }

                // Switch to the backup database
                //dbContext.Database = backupDbContext.Database;
                //dbContext.SaveChanges();
                var databaseField = typeof(DbContext).GetField("_database", BindingFlags.NonPublic | BindingFlags.Instance);
                databaseField.SetValue(this, backupDbContext.Database);
                //dbContext = backupDbContext;
            }

            await _next(context);
        }
    }
}
