using Newtonsoft.Json;

namespace Models.Movies
{
    public class Genre
    {
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }
    }
}