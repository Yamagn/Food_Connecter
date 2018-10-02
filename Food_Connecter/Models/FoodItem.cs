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
    }
}
