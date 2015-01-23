using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MoviePickerApi.Models;
using MoviePickerApi.Models.Models;
using TMDbLib.Client;

namespace MoviePickerApi.Repositories
{
    public class MovieRepository
    {
        private readonly MoviePickerContext _db = new MoviePickerContext();
        private const string ApiKey = "13f441b40fdb830f1f661b50a634304d";
        private readonly TMDbClient _client = new TMDbClient(ApiKey);

        public MovieRepository()
        {
            _client.GetConfig();
        }

        public void LikeMovie(int userId, int movieId)
        {
            // retrieve movie from database
            // increment genres, actors, language and year
            // write to database
            // indicate movie has been rated by this user
            AdjustRating(userId, movieId, +1);
        }

        public void DislikeMovie(int userId, int movieId)
        {
            // retrieve movie from database
            // decrement genres, actors, language and year
            // write to database
            // indicate movie has been rated by this user
            AdjustRating(userId, movieId, -1);
        }

        public IEnumerable<Movie> GetMovies(int userId)
        {
            // get all preferences from user
            // order preferences by rating
            // search for movie constrained by the first preference
            // search for movie constrained by the first and second preferences
            // search for movie constrained by (year - 5), (year + 5) and first genre
            // For each movie that isn't inside the database: add it

            const int maxResults = 3;
            var genrePreferences = _db.GenrePreferences.Where(x => x.UserId == userId)
                                      .OrderByDescending(x => x.Rating)
                                      .Take(maxResults)
                                      .Select(x => _db.Genres.FirstOrDefault(y => y.Id == x.GenreId));

            var pivotYear = _db.YearPreferences.Where(x => x.UserId == userId)
                               .OrderByDescending(x => x.Rating)
                               .First()
                               .Year;
            // TODO: what was I doing here?
            var minYear = pivotYear - 5;
            var maxYear = pivotYear + 5;

            var firstMovieId = _client.DiscoverMovies(page: 1, withGenres: genrePreferences.First().Name).Results.First().Id;
            yield return Mapper.Map<Movie>(_client.GetMovie(firstMovieId));
        }

        private void AdjustRating(int userId, int movieId, int change)
        {
            var genrePreferences = _db.GenrePreferences
                                      .Where(x => x.UserId == userId)
                                      .ToArray();
            var yearPreferences = _db.YearPreferences
                                     .Where(x => x.UserId == userId)
                                     .ToArray();

            var movie = _db.Movies.Find(movieId);
            foreach (var genre in movie.Genres)
            {
                var currentGenre = genrePreferences.First(x => x.GenreId == genre.Id);
                currentGenre.Rating += change;
            }

            if (movie.ReleaseDate.HasValue)
            {
                var year = movie.ReleaseDate.Value.Year;
                var selectedYear = yearPreferences.First(x => x.Year == year);
                selectedYear.Rating += change;
            }
            _db.SaveChanges();
        }
    }
}