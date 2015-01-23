using Newtonsoft.Json;

namespace Models.Movies
{
    public class Language
    {
        [JsonProperty("iso_639_1")]
        public string Iso { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}