using Newtonsoft.Json;

namespace Models.Movies
{
    public class Keyword
    {
        public Keyword()
        {
        }

        public Keyword(int id, string name)
        {
            Id = id;
            Name = name;
        }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}