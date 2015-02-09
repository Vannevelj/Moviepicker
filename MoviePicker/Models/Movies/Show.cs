using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Models.Movies
{
    public class Show
    {
        [JsonProperty("backdrop_path")]
        public string BackdropPath { get; set; }

        [JsonProperty("first_air_date")]
        public DateTime FirstAiring { get; set; }

        [JsonProperty("last_air_date")]
        public DateTime LastAiring { get; set; }

        [JsonProperty("homepage")]
        public string Homepage { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("in_production")]
        public bool? InProduction { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("number_of_episodes")]
        public int? AmountOfEpisodes { get; set; }

        [JsonProperty("number_of_seasons")]
        public int? AmountOfSeasons { get; set; }

        [JsonProperty("original_language")]
        public string OriginalLanguage { get; set; }

        [JsonProperty("original_name")]
        public string OriginalName { get; set; }

        [JsonProperty("overview")]
        public string Overview { get; set; }

        [JsonProperty("popularity")]
        public double? Popularity { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("vote_average")]
        public double? AverageVote { get; set; }

        [JsonProperty("vote_count")]
        public int? AmountOfVotes { get; set; }

        /// <summary>
        /// This might need to be changed. TV languages are returned as simple "en" strings while movie languages return the ISO code and the name
        /// </summary>
        [JsonProperty("homepage")]
        public ICollection<string> Languages { get; set; }

        [JsonProperty("genres")]
        public ICollection<Genre> Genres { get; set; }

        [JsonProperty("backdrops")]
        public virtual ICollection<ImageInfo> Backdrops { get; set; }

        [JsonProperty("posters")]
        public virtual ICollection<ImageInfo> Posters { get; set; } 
    }
}
