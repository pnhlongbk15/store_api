using MySql.Data.MySqlClient;

namespace Store
{
    public static class FilterConnectionString
    {
        private static List<String> _connStrings = new List<string>();
        private static IConfiguration _configuration = new ConfigurationBuilder()
                                                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                                        .Build();
        static FilterConnectionString()
        {
            List<string> connectionStringKeys = new List<string> { "MyStore", "Backup" };

            foreach (var key in connectionStringKeys)
            {
                _connStrings.Add(_configuration.GetConnectionString(key));
            }
        }

        public static string ConnectionString()
        {
            string result = null;

            foreach (var connString in _connStrings)
            {
                if (CheckConnection(connString))
                {
                    Console.WriteLine(connString);
                    result = connString;
                    break;
                }
            }
            return result;
        }

        private static bool CheckConnection(string connectString)
        {
            MySqlConnection connection = new MySqlConnection(connectString);
            try
            {
                connection.Open();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally { connection.Close(); }
        }

    }
}
