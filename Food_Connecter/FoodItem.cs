using System.Collections.Generic;
using Newtonsoft.Json;
using SQLite;
namespace Food_Connecter
{
    //[JsonObject]
    public class FoodItem
    {
        //[JsonArray]
        //public class imageData
        //{
        //    [JsonProperty("images")]
        //    public classifiers[] Images { get; set; }
        //}


        //[JsonArray]
        //public class classifiers
        //{
        //    [JsonProperty("classifiers")]
        //    public classes[] Classifiers { get; set; }
        //}
        //[JsonObject]
        //public class classes
        //{
        //    [PrimaryKey, AutoIncrement]
        //    public int ID { get; set; }
        //    [JsonProperty("class")]
        //    public string Class { get; set; }
        //    [JsonProperty("score")]
        //    public int Score { get; set; }
        //}
        [JsonProperty(PropertyName = "images")]
        public List<Image> Images { get; set; }
    }

    public class Image
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
