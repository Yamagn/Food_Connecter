using System;
using Newtonsoft.Json;

namespace Food_Connecter
{
    public class osusowakeItem
    {
        string username;
        string food;
        string image_url;
        string info;
        DateTime fooddate;
        int foodnum;

        [JsonProperty(PropertyName = "username")]
        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        [JsonProperty(PropertyName = "food")]
        public string Food
        {
            get { return food; }
            set { food = value; }
        }

        [JsonProperty(PropertyName = "image_url")]
        public string Image_url
        {
            get { return image_url; }
            set { image_url = value; }
        }

        [JsonProperty(PropertyName = "info")]
        public string Info
        {
            get { return info; }
            set { info = value; }
        }

        [JsonProperty(PropertyName = "fooddate")]
        public DateTime Fooddate
        {
            get { return fooddate; }
            set { fooddate = value; }
        }

        [JsonProperty(PropertyName = "foodnum")]
        public int Foodnum
        {
            get { return foodnum; }
            set { foodnum = value; }
        }
    }
}
