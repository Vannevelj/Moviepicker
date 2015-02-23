using Newtonsoft.Json;

namespace Models.Movies
{
    public class Genre
    {
        /// <summary>
        ///     This ID is used for internal representation in the database
        /// </summary>
        [JsonIgnore]
        public int Id { get; set; }

        [JsonProperty("id")]
        public int TMDbId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        public Genre()
        {
            
        }

        public Genre(int tmdbId, string name)
        {
            TMDbId = tmdbId;
            Name = name;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Genre))
            {
                return false;
            }

            return ((Genre) obj).TMDbId == TMDbId;
        }

        public override int GetHashCode()
        {
            return TMDbId.GetHashCode();
        }
    }
}