using System;
using System.Collections.Generic;
using Newtonsoft.Json;
namespace Food_Connecter
{
    public class ManageEvent
    {
        [JsonProperty("eventname")]
        public string eventName { get; set; }
        [JsonProperty("eventplace")]
        public string eventPlace { get; set; }
        [JsonProperty("eventcity")]
        public string eventCity { get; set; }
        [JsonProperty("date")]
        public string Date { get; set; }
        [JsonProperty("menbers")]
        public List<menber> Menbers { get; set; }
        [JsonProperty("food")]
        public List<food> Foods { get; set; }
        [JsonProperty("wanted")]
        public List<wanted> Wanteds { get; set; }
    }
    public class menber
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("username")]
        public string userName { get; set; }
    }
    public class food
    {
        [JsonProperty("username")]
        public string userName { get; set; }
        [JsonProperty("foodname")]
        public string foodName { get; set; }
    }
    public class wanted
    {
        [JsonProperty("foodname")]
        public string foodName { get; set; }
    }
}
