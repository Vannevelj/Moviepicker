using System;
using System.Collections.Generic;
using Models.Movies;

namespace Tests.TestUtilities
{
    internal static class TestDataProvider
    {
        public static IEnumerable<ImageInfo> GetBackdrops()
        {
            yield return new ImageInfo
            {
                AspectRatio = 1.77,
                Path = "/nnMC0BM6XbjIIrT4miYmMtPGcQV.jpg",
                Height = 1080,
                IsoCode = "xx",
                AverageVote = 5.28,
                Width = 1920
            };
        }

        public static IEnumerable<ImageInfo> GetPosters()
        {
            yield return new ImageInfo
            {
                AspectRatio = 1.52,
                Path = "/nmiYmMtPGcQV.jpg",
                Height = 720,
                IsoCode = "xx",
                AverageVote = 7.36,
                Width = 1080,
                TMDbId = "7891"
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
                PosterPath = "/w0NzAc4Lv6euPtPAmsdEf0ZCF8C.jpg"
            };
        }

        public static IEnumerable<Language> GetLanguages()
        {
            yield return new Language("en-US", "US English");
            yield return new Language("nl-BE", "Nederlands");
            yield return new Language("fr-FR", "Français");
        }
    }
}