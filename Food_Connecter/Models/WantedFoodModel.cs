using System;
using System.Collections.Generic;
using Newtonsoft.Json;
namespace Food_Connecter
{
    public class WantedFoodModel
    {
        [JsonProperty("id")]
        public string userId { get; set; }
        [JsonProperty("eventnum")]
        public int eventNum { get; set; }
        [JsonProperty("food")]
        public string wanteds { get; set; }
    }
}
