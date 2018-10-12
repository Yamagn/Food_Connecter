using System;
using Newtonsoft.Json;

namespace Food_Connecter
{
    public class viewEventModel
    {
        [JsonProperty("num")]
        public string eventNum { get; set; }
        [JsonProperty("eventname")]
        public string eventName { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("place")]
        public string Place { get; set; }
    }
}
