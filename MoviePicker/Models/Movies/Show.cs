using System;
using System.Collections.Generic;
using Models.Utilities;
using Newtonsoft.Json;

namespace Models.Movies
{
    public class Show
    {
        public Show()
        {
            Languages = new List<Language>();
            Genres = new List<Genre>();
            Posters = new List<PosterImageInfo>();
            Backdrops = new List<BackdropImageInfo>();
        }

        [JsonProperty("backdrop_path")]
        public string BackdropPath { get; set; }

        [JsonProperty("first_air_date")]
        public DateTime? FirstAiring { get; set; }

        [JsonProperty("last_air_date")]
        public DateTime? LastAiring { get; set; }

        [JsonProperty("homepage")]
        public string Homepage { get; set; }

        [JsonProperty("id")]
        public int TmdbId { get; set; }

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

        [JsonProperty("added_on")]
        public DateTime AddedOn { get; set; }

        [JsonProperty("last_updated_on")]
        public DateTime? LastUpdatedOn { get; set; }

        [JsonProperty("languages")]
        [JsonConverter(typeof (LanguageConverter))]
        public virtual ICollection<Language> Languages { get; set; }

        [JsonProperty("genres")]
        public virtual ICollection<Genre> Genres { get; set; }

        [JsonProperty("backdrops")]
        public virtual ICollection<BackdropImageInfo> Backdrops { get; set; }

        [JsonProperty("posters")]
        public virtual ICollection<PosterImageInfo> Posters { get; set; }

        public override bool Equals(object obj)
        {
            var otherMovie = obj as Show;
            if (otherMovie == null)
            {
                return false;
            }

            return otherMovie.TmdbId == TmdbId;
        }

        public override int GetHashCode()
        {
            return TmdbId.GetHashCode();
        }

        public void Update(Show show)
        {
            
        }
    }
}