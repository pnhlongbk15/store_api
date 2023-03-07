using Microsoft.EntityFrameworkCore;

namespace Store.Services
{
    public static class LoggerService
    {
        public static ILoggerFactory Factory = LoggerFactory.Create(builder =>
        {
            builder.AddFilter(DbLoggerCategory.Query.Name, LogLevel.Information);
            //builder.AddFilter(DbLoggerCategory.Database.Name, LogLevel.Information);
            builder.AddConsole();
        });
    }
}
