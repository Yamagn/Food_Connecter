using System;
using Newtonsoft.Json;
namespace Food_Connecter
{
    public class getChatModel
    {
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("flag")]
        public bool Flag { get; set; }
        [JsonProperty("datetime")]
        public string datetime { get; set; }
        [JsonProperty("username")]
        public string userName { get; set; }
    }
}
