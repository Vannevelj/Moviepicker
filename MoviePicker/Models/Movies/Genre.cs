using Newtonsoft.Json;

namespace Models.Movies
{
    public class Genre
    {
        public Genre()
        {
        }

        public Genre(int tmdbId, string name)
        {
            TmdbId = tmdbId;
            Name = name;
        }

        [JsonProperty("id")]
        public int TmdbId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is Genre))
            {
                return false;
            }

            return ((Genre) obj).TmdbId == TmdbId;
        }

        public override int GetHashCode()
        {
            return TmdbId.GetHashCode();
        }
    }
}