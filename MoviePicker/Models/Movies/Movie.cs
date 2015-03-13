using System;
using System.Collections.Generic;
using System.Linq;
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

        public virtual void Update(Movie movie)
        {
            ImdbId = movie.ImdbId;
            Title = movie.Title;
            OriginalTitle = movie.OriginalTitle;
            Status = movie.Status;
            Tagline = movie.Tagline;
            Overview = movie.Overview;
            BackdropPath = movie.BackdropPath;
            PosterPath = movie.PosterPath;
            Adult = movie.Adult;
            ReleaseDate = movie.ReleaseDate;
            Revenue = movie.Revenue;
            Budget = movie.Budget;
            Runtime = movie.Runtime;
            Popularity = movie.Popularity;
            VoteAverage = movie.VoteAverage;
            VoteCount = movie.VoteCount;
            AddedOn = movie.AddedOn;
            LastUpdatedOn = movie.LastUpdatedOn;

            foreach (var genre in movie.Genres)
            {
                var existingGenre = Genres.SingleOrDefault(x => x.TmdbId == genre.TmdbId);
                if (existingGenre == null)
                {
                    Genres.Add(genre);
                }
                else
                {
                    existingGenre.Update(genre);
                }
            }

            foreach (var keyword in movie.Keywords)
            {
                var existingKeyword = Keywords.SingleOrDefault(x => x.Id == keyword.Id);
                if (existingKeyword == null)
                {
                    Keywords.Add(keyword);
                }
                else
                {
                    existingKeyword.Update(keyword);
                }
            }

            foreach (var backdrop in movie.Backdrops)
            {
                var existingBackdrop = Backdrops.SingleOrDefault(x => x.Id == backdrop.Id);
                if (existingBackdrop == null)
                {
                    Backdrops.Add(backdrop);
                }
                else
                {
                    existingBackdrop.Update(backdrop);
                }
            }

            foreach (var poster in movie.Posters)
            {
                var existingPoster = Posters.SingleOrDefault(x => x.Id == poster.Id);
                if (existingPoster == null)
                {
                    Posters.Add(poster);
                }
                else
                {
                    existingPoster.Update(poster);
                }
            }

            foreach (var language in movie.Languages)
            {
                var existingLanguage = Languages.SingleOrDefault(x => x.Iso == language.Iso);
                if (existingLanguage == null)
                {
                    Languages.Add(language);
                }
                else
                {
                    existingLanguage.Update(language);
                }
            }
        }
    }
}