using System;
using Newtonsoft.Json;
namespace Food_Connecter
{
    public class postEvent
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("eventname")]
        public string eventName { get; set; }
        [JsonProperty("pref")]
        public string Pref { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("place")]
        public string Place { get; set; }
        [JsonProperty("eventdate")]
        public string eventDate { get; set; }
    }
}
