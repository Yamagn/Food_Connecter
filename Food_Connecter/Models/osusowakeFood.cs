using System;
using Newtonsoft.Json;
namespace Food_Connecter
{
    public class osusowakeFood
    {
        public int ID { get; set; }
        [JsonProperty("username")]
        public string userName { get; set; }
        [JsonProperty("food")]
        public string Food { get; set; }
        [JsonProperty("image_url")]
        public string imageUrl { get; set; }
        [JsonProperty("info")]
        public string Info { get; set; }
        [JsonProperty("fooddate")]
        public string foodDate { get; set; }
        [JsonProperty("foodnum")]
        public int foodNum { get; set; }
        [JsonProperty("id")]
        public string userId { get; set; }
    }
}
