using System.Collections.Generic;
using Models.Movies;

namespace Tests.Utilities
{
    public static class TestDataProvider
    {
        public static IEnumerable<Genre> GetGenres()
        {
            yield return new Genre
            {
                TMDbId = 15,
                Name = "Horror"
            };

            yield return new Genre
            {
                TMDbId = 8942,
                Name = "Comedy"
            };

            yield return new Genre
            {
                TMDbId = 1234,
                Name = "Drama"
            };

            yield return new Genre
            {
                TMDbId = 2345,
                Name = "War"
            };

            yield return new Genre
            {
                TMDbId = 3456,
                Name = "Cartoon"
            };

            yield return new Genre
            {
                TMDbId = 4567,
                Name = "History"
            };
        }
    }
}