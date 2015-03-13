using System;
using System.Collections.Generic;
using Models.Movies;

namespace Tests.TestUtilities
{
    internal static class TestDataProvider
    {
        public static IEnumerable<BackdropImageInfo> GetBackdrops()
        {
            yield return new BackdropImageInfo
            {
                AspectRatio = 1.77,
                Path = "/nnMC0BM6XbjIIrT4miYmMtPGcQV.jpg",
                Height = 1080,
                IsoCode = "xx",
                AverageVote = 5.28,
                Width = 1920,
                Id = 85,
                VoteCount = 21
            };
        }

        public static IEnumerable<PosterImageInfo> GetPosters()
        {
            yield return new PosterImageInfo
            {
                AspectRatio = 1.52,
                Path = "/nmiYmMtPGcQV.jpg",
                Height = 720,
                IsoCode = "xx",
                AverageVote = 7.36,
                Width = 1080,
                TdmbId = "7891",
                Id = 89,
                VoteCount = 5
            };
        }

        public static IEnumerable<Keyword> GetKeywords()
        {
            yield return new Keyword(150, "Prison");
            yield return new Keyword(850, "underdog");
        }

        public static Movie GetMovie()
        {
            return new Movie
            {
                TmdbId = 987,
                ImdbId = "tt0094675",
                Adult = false,
                Budget = 157845,
                AddedOn = new DateTime(2012, 07, 06),
                LastUpdatedOn = new DateTime(2013, 07, 25),
                Popularity = 12.3,
                Overview = "Overview",
                Title = "Sample Title",
                OriginalTitle = "Title Sample",
                Revenue = 789456123,
                ReleaseDate = new DateTime(2013, 08, 09),
                Runtime = 93,
                Status = "Released",
                Tagline = "Sample Tagline",
                VoteAverage = 8.6,
                VoteCount = 999,
                PosterPath = "/w0NzAc4Lv6euPtPAmsdEf0ZCF8C.jpg",
                BackdropPath = "/sdqsw0NzAc4Lv6euPtPAmsjhfgf0ZCF8C.jpg"
            };
        }

        public static IEnumerable<Language> GetLanguages()
        {
            yield return new Language("en-US", "US English");
            yield return new Language("nl-BE", "Nederlands");
            yield return new Language("fr-FR", "Français");
        }

        public static IEnumerable<Genre> GetGenres()
        {
            yield return new Genre(148, "Horror");
            yield return new Genre(8411, "Comedy");
            yield return new Genre(974, "Adventure");
        }

        public static Show GetShow()
        {
            return new Show
            {
                TmdbId = 987,
                AddedOn = new DateTime(2012, 07, 06),
                LastUpdatedOn = new DateTime(2013, 07, 25),
                Popularity = 12.3,
                Overview = "Overview",
                Status = "Released",
                Name = "Test name",
                Homepage = "http://myshow.com",
                FirstAiring = new DateTime(2010, 05, 07),
                LastAiring = new DateTime(2015, 01, 25),
                AmountOfEpisodes = 14,
                AmountOfSeasons = 3,
                AmountOfVotes = 204,
                AverageVote = 6.8,
                BackdropPath = "/w0NzAc4LPtPAmsdEf0ZCF8C.jpg",
                Type = "something",
                InProduction = true,
                OriginalLanguage = "en-US",
                OriginalName = "La vida loca"
            };
        }
    }
}