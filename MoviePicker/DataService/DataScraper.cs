using System;
using System.Linq;
using System.Threading.Tasks;
using Database.Repositories.Declarations;
using TMDbWrapper;

namespace DataService
{
    internal class DataScraper
    {
        private readonly TMDbApi _api;
        private readonly IMovieRepository _movieRepository;

        internal DataScraper(string apiKey, IMovieRepository movieRepository)
        {
            _api = new TMDbApi(apiKey);
            _movieRepository = movieRepository;
        }

        internal async Task UpdateGenresAsync()
        {
            var showGenres = await _api.GetShowGenresAsync();
            if (showGenres.IsSuccess)
            {
                Console.WriteLine("Found {0} shows", showGenres.Data.Count());
                foreach (var genre in showGenres.Data)
                {
                    _movieRepository.InsertOrUpdate(genre);
                }
            }
            else
            {
                Console.WriteLine("Error occurred retrieving show genres: " + showGenres.StatusCode);
            }

            var movieGenres = await _api.GetMovieGenresAsync();
            if (movieGenres.IsSuccess)
            {
                Console.WriteLine("Found {0} movies", movieGenres.Data.Count());
                foreach (var genre in movieGenres.Data)
                {
                    _movieRepository.InsertOrUpdate(genre);
                }
            }
            else
            {
                Console.WriteLine("Error occurred retrieving movie genres: " + showGenres.StatusCode);
            }
        }
    }
}