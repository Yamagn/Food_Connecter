using System;
using System.Collections.Generic;
using Newtonsoft.Json;
namespace Food_Connecter
{
    public class eventDetail
    {
        [JsonProperty("eventname")]
        public string eventName { get; set; }
        [JsonProperty("eventplace")]
        public string eventPlace { get; set; }
        [JsonProperty("eventcity")]
        public string eventCity { get; set; }
        [JsonProperty("date")]
        public string Date { get; set; }
        [JsonProperty("food")]
        public List<food> Foods { get; set; }
        [JsonProperty("wanted")]
        public List<wanted> Wanteds { get; set; }
    }
}
