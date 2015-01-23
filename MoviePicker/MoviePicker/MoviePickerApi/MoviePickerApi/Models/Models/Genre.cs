using Newtonsoft.Json;

namespace MoviePickerApi.Models.Models
{
    public class Genre
    {
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }
    }
}