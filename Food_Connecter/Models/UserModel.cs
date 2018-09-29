using System;
using Newtonsoft.Json;
namespace Food_Connecter
{
    public class UserModel
    {
        [JsonProperty("id")]
        public string UserID { get; set; }
        [JsonProperty("username")]
        public string UserName { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("pref")]
        public string Pref { get; set; }
    }
}
