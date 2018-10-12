using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UIKit;

using Xamarin.Forms;

namespace Food_Connecter
{
    public class CognitiveAPIClient
    {
        public static async Task<FoodItem> AnalizeAsync(string photoURL)
        {
            try
            {
                var serverUri = Constants.ApplicationURL + "/api/foodlearn";
                var content = new StreamContent(File.OpenRead(photoURL));
                HttpResponseMessage res = App.client.PostAsync(serverUri, content).Result;
                Console.WriteLine(res.StatusCode);
                var jsontext = await res.Content.ReadAsStringAsync();
                var ps = JsonConvert.DeserializeObject<FoodItem>(jsontext);
                foreach(var i in ps.Images[0].Classifiers[0].Classes)
                {
                    i.Date = DateTime.Now.AddDays(4);
                }
                return ps;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
