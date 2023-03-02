namespace Store
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHostBuilder builder = Host.CreateDefaultBuilder(args);
            builder.ConfigureWebHostDefaults(configs =>
            {
                configs.UseStartup<Startup>();
            });

            builder.Build().Run();
        }
    }
}