using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiClient
{ 
    public class Program
    {
        static async Task Main(string[] args)
        {
            var id = args.Length > 0 ? args[0] : "1";
            using var client = new HttpClient();
            var json = await client.GetStringAsync($"https://oda.ft.dk/api/Afstemning({id})");
            Console.WriteLine(json);
        }
    }
}