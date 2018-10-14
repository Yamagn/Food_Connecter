using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SQLite;

namespace Food_Connecter
{
    public class FoodItem
    {
        [JsonProperty(PropertyName = "images")]
        public List<Images> Images { get; set; }
    }

    public class Images
    {
        [JsonProperty(PropertyName = "classifiers")]
        public List<Classifier> Classifiers { get; set; }
    }

    public class Classifier
    {
        [JsonProperty(PropertyName = "classes")]
        public List<ClassData> Classes { get; set; }
    }

    public class ClassData
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [JsonProperty(PropertyName = "class")]
        public string Class { get; set; }

        [JsonProperty(PropertyName = "score")]
        public string Score { get; set; }

        public DateTime Date { get; set; }

        public string Limit { get; set; }

        public bool IsOsusowake { get; set; } = false;

        public bool IsOffer { get; set; } = false;

        public string image { get; set; }

        public string Quantity { get; set; }
    }

    public class foodNum
    {
        [JsonProperty("foodnum")]
        public string FoodNum { get; set; }
    }
}
