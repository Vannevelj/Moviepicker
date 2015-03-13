using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Models.Movies
{
    public class Movie
    {
        public Movie()
        {
            Languages = new List<Language>();
            Genres = new List<Genre>();
            Keywords = new List<Keyword>();
            Backdrops = new List<BackdropImageInfo>();
            Posters = new List<PosterImageInfo>();
        }

        [JsonProperty("id")]
        public int TmdbId { get; set; }

        [JsonProperty("imdb_id")]
        public string ImdbId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("original_title")]
        public string OriginalTitle { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("tagline")]
        public string Tagline { get; set; }

        [JsonProperty("overview")]
        public string Overview { get; set; }

        [JsonProperty("backdrop_path")]
        public string BackdropPath { get; set; }

        [JsonProperty("poster_path")]
        public string PosterPath { get; set; }

        [JsonProperty("adult")]
        public bool? Adult { get; set; }

        [JsonProperty("release_date")]
        public DateTime? ReleaseDate { get; set; }

        [JsonProperty("revenue")]
        public long? Revenue { get; set; }

        [JsonProperty("budget")]
        public long? Budget { get; set; }

        [JsonProperty("runtime")]
        public int? Runtime { get; set; }

        [JsonProperty("popularity")]
        public double? Popularity { get; set; }

        [JsonProperty("vote_average")]
        public double? VoteAverage { get; set; }

        [JsonProperty("vote_count")]
        public int? VoteCount { get; set; }

        [JsonProperty("added_on")]
        public DateTime AddedOn { get; set; }

        [JsonProperty("last_updated_on")]
        public DateTime? LastUpdatedOn { get; set; }

        [JsonProperty("spoken_languages")]
        public virtual ICollection<Language> Languages { get; set; }

        [JsonProperty("genres")]
        public virtual ICollection<Genre> Genres { get; set; }

        [JsonProperty("keywords")]
        public virtual ICollection<Keyword> Keywords { get; set; }

        [JsonProperty("backdrops")]
        public virtual ICollection<BackdropImageInfo> Backdrops { get; set; }

        [JsonProperty("posters")]
        public virtual ICollection<PosterImageInfo> Posters { get; set; }

        public override bool Equals(object obj)
        {
            var otherMovie = obj as Movie;
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

    }
}