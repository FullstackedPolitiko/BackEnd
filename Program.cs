using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiClient
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var api = new FtApiClient();

            if (args.Length == 0)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("dotnet run <sagid>");
                return;
            }

            if (!int.TryParse(args[0], out int sagid))
            {
                Console.WriteLine("Invalid sagid");
                return;
            }

            try
            {
                Sag? sag = await api.GetSagAsync(sagid);
                if (sag == null)
                {
                    Console.WriteLine("Sag not found");
                    return;
                }

                string prettyJson = JsonSerializer.Serialize(sag, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                Console.WriteLine(prettyJson);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }
    }
}