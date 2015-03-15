using Newtonsoft.Json;

namespace Models.Movies
{
    public abstract class ImageInfo
    {
        [JsonIgnore]
        public int Id { get; set; }

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
    }

    public class PosterImageInfo : ImageInfo
    {
        [JsonProperty("id")]
        public string TmdbId { get; set; }
    }

    public class BackdropImageInfo : ImageInfo
    {
    }
}