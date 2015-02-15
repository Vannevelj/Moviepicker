using Newtonsoft.Json;

namespace Models.Movies
{
    public class ImageInfo
    {
        [JsonProperty("aspect_ratio")]
        public double? AspectRatio { get; set; }

        [JsonProperty("file_path")]
        public string Path { get; set; }

        [JsonProperty("height")]
        public int? Height { get; set; }

        [JsonProperty("width")]
        public int? Width { get; set; }

        [JsonProperty("iso_639_1")]
        public string IsoCode { get; set; }

        [JsonProperty("vote_average")]
        public double? AverageVote { get; set; }

        [JsonProperty("vote_count")]
        public int? VoteCount { get; set; }

        /// <summary>
        ///     Only used for posters
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}