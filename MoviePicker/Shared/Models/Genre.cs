using Newtonsoft.Json;

namespace Shared.Models
{
    public class Genre
    {
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }
    }
}
