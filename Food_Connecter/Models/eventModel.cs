using System;
using Newtonsoft.Json;
namespace Food_Connecter
{
    public class eventModel
    {
        [JsonProperty("eventnum")]
        public int Num { get; set; }
        [JsonProperty("eventname")]
        public string eventName { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("eventplace")]
        public string Place { get; set; }
        [JsonProperty("eventdate")]
        public string Date { get; set; }
        [JsonProperty("userid")]
        public string Userid { get; set; }
    }
}
