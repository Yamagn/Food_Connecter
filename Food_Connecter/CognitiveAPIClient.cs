using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Food_Connecter
{
    public class CognitiveAPIClient
    {
        public static async Task<string> AnalizeAsync(string photoURL)
        {
            var client = new System.Net.Http.HttpClient();
            var content = new MultipartFormDataContent(photoURL);
            var result = await client.PostAsync("https://samplefood.azurewebsites.net/api/foodlearn", content);
            System.Console.WriteLine(result);
            return "success";
        }
    }
}
