// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;

//I will call the API GetServices from my home controller to display some info on the products such as: ID, name, Price, Path (image), Product ID, Type ID.
namespace SalonConsoleClient
{
    class Program
    {
        static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            try
            {
                //Add my URL endpoint - GetServices API created in my Home Controller
                string url = "https://localhost:7005/Home/GetServices";
                var response = await client.GetAsync(url);
                //check it is successful - if not an exception code will be thrown
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject(responseBody);

                //Print the info 
                Console.WriteLine("Received data:");
                Console.WriteLine(JsonConvert.SerializeObject(data, Formatting.Indented));
            }
            catch (HttpRequestException e)
            {
                //Print any errors if exception happoens
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }
    }
}
