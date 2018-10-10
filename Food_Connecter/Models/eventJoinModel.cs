using System;
using Newtonsoft.Json;
namespace Food_Connecter
{
    public class eventJoinModel
    {
        [JsonProperty("id")]
        public string userId { get; set; }
        [JsonProperty("eventnum")]
        public int eventNum { get; set; }
    }
}
