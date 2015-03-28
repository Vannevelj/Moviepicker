using Newtonsoft.Json;

namespace Models.ApiResponses
{
    public struct Change
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("adult")]
        public bool? IsAdult { get; set; }
    }
}