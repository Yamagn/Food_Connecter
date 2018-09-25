using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UIKit;

namespace Food_Connecter
{
    public class CognitiveAPIClient
    {
        public static async Task<FoodItem> AnalizeAsync(string photoURL)
        {
            try
            {
                //var beareToken = "Bearer " + App.Authenticator.user.MobileServiceAuthenticationToken;
                //Console.WriteLine(beareToken);
                var client = new HttpClient();
                var serverUri = "https://samplefood.azurewebsites.net/api/foodlearn";
                var content = new StreamContent(File.OpenRead(photoURL));
                //client.DefaultRequestHeaders.Add("Authorization",beareToken);
                HttpResponseMessage res = client.PostAsync(serverUri, content).Result;
                Console.WriteLine(res.StatusCode);
                var jsontext = await res.Content.ReadAsStringAsync();
                UIAlertView avAlert = new UIAlertView("responseresult", jsontext, null, "OK", null);
                avAlert.Show();
                var ps = JsonConvert.DeserializeObject<FoodItem>(jsontext);
                //await App.Database.SaveItemAsync(ps);
                //ObservableCollection<FoodItem> Foods = new ObservableCollection<FoodItem>(ps);
                foreach (var v in ps.Images[0].Classifiers[0].Classes)
                {
                    Console.WriteLine("{0}:{1}:{2}", v.ID, v.Class, v.Score);
                    await App.Database.SaveItemAsync(v);
                }
                return ps;

            }
            catch (Exception e)
            {
                UIAlertView avAlert = new UIAlertView("Error", e.Message, null, "OK", null);
                avAlert.Show();
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
