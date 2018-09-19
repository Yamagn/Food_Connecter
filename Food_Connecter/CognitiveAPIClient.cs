using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using PCLStorage;

namespace Food_Connecter
{
    public class CognitiveAPIClient
    {
        public static async Task<HttpStatusCode> AnalizeAsync(string photoURL)
        {
            var client = new HttpClient();
            var file = await FileSystem.Current.GetFileFromPathAsync(photoURL);
            var imageStream = await file.OpenAsync(FileAccess.Read);
            var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(imageStream);
            content.Add(fileContent, "image");
            var result = await client.PostAsync("http://samplefood.azurewebsites.net/api/foodlearn", content);
            return result.StatusCode;
        }
    }
}
