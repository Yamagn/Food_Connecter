using System;
using Newtonsoft.Json;
namespace Food_Connecter
{
    public class postFood
    {
        [JsonProperty("userid")]
        public string userId { get; set; }
        [JsonProperty("food")]
        public string foodName { get; set; }
        [JsonProperty("fooddate")]
        public string foodDate { get; set; }
        [JsonProperty("info")]
        public string Info { get; set; }
        [JsonProperty("foodnum")]
        public int foodNum { get; set; }
    }
}
