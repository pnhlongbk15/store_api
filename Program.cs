using StoreApi.Utils;

namespace Store
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateExampleData.GetProducts();

            IHostBuilder builder = Host.CreateDefaultBuilder(args);
            builder.ConfigureWebHostDefaults(configs =>
            {
                configs.UseStartup<Startup>();
            });

            builder.Build().Run();
        }
    }
}