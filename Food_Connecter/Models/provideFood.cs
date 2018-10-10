using System;
using Newtonsoft.Json;
namespace Food_Connecter
{
    public class provideFood
    {
        [JsonProperty("id")]
        public string userId { get; set; }
        [JsonProperty("eventnum")]
        public int Num { get; set; }
        [JsonProperty("food")]
        public string Food { get; set; }
    }
}
