using System;
using System.Collections.Generic;

namespace Models.Movies
{
    public class Movie
    {
        public int Id { get; set; }
        public string ImdbId { get; set; }
        public string Title { get; set; }
        public string OriginalTitle { get; set; }
        public string Status { get; set; }
        public string Tagline { get; set; }
        public string Overview { get; set; }
        public string BackdropPath { get; set; }
        public string PosterPath { get; set; }
        public bool Adult { get; set; }
        public List<Genre> Genres { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public long Revenue { get; set; }
        public long Budget { get; set; }
        public int? Runtime { get; set; }
        public double Popularity { get; set; }
        public double VoteAverage { get; set; }
        public int VoteCount { get; set; }

        //public List<SpokenLanguage> SpokenLanguages { get; set; }
        //public KeywordsContainer Keywords { get; set; }
        //public SearchContainer<MovieResult> SimilarMovies { get; set; }
    }
}