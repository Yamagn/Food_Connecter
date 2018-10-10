using System;
using Newtonsoft.Json;
namespace Food_Connecter
{
    public class postChatModel
    {
        [JsonProperty("user")]
        public string User { get; set; }
        [JsonProperty("touser")]
        public string ToUser { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
