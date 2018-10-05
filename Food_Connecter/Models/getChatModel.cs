using System;
using Newtonsoft.Json;
namespace Food_Connecter
{
    public class getChatModel
    {
        [JsonProperty("username")]
        public string userName { get; set; }
        [JsonProperty("flag")]
        public bool Flag { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("datetime")]
        public string datetime { get; set; }
    }
}
