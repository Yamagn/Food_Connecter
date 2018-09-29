using System;
using Newtonsoft.Json;
namespace Food_Connecter
{
    public class TownData
    {
        [JsonProperty("city_id")]
        public string townID { get; set; }
        [JsonProperty("pref_name")]
        public string prefName { get; set; }
        [JsonProperty("city_name")]
        public string townName { get; set; }
    }
}
