using RestSharp;

namespace StoreApi.Utils
{
    public static class CreateExampleData
    {
        public static void GetProducts()
        {
            RestClient client = new RestClient();

            RestRequest request = new RestRequest("https://alpha-vantage.p.rapidapi.com/query?interval=5min&function=TIME_SERIES_INTRADAY&symbol=MSFT&datatype=json&output_size=compact", Method.Get);
            request.AddHeader("X-RapidAPI-Key", "944f021263msh3ef8fab96844873p1ff60cjsneff7ad8cf1eb");
            request.AddHeader("X-RapidAPI-Host", "alpha-vantage.p.rapidapi.com");

            RestResponse response = client.Execute(request);

            if (response.IsSuccessful)
            {
                // Print the response content to the console
                Console.WriteLine(response.Content);
            }
            else
            {
                Console.WriteLine("Error fetching data. Status code: " + response.StatusCode);
            }

        }
    }
}
