using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using PCLStorage;

namespace Food_Connecter
{
    public class CognitiveAPIClient
    {
        public static async Task<HttpResponseMessage> AnalizeAsync(string photoURL)
        {
            try
            {
                var client = new HttpClient();
                var serverUri = "https://samplefood.azurewebsites.net/api/foodlearn";
                var content = new StreamContent(File.OpenRead(photoURL));
                HttpResponseMessage res = client.PostAsync(serverUri, content).Result;
                Console.WriteLine(await res.Content.ReadAsStringAsync());
                return res;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
