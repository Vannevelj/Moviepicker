using System;
using System.Linq;
using System.Threading.Tasks;
using Database.Repositories.Declarations;
using Models.Movies;
using Models.Utilities;
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
            var showResults = await _api.GetShowGenresAsync();
            if (showResults.IsSuccess)
            {
                Console.WriteLine("Found {0} shows", showResults.Data.Count());
                foreach (var genre in showResults.Data)
                {
                    _movieRepository.InsertOrUpdate(genre);
                }
            }
            else
            {
                Console.WriteLine("Error occurred retrieving show genres: " + showResults.StatusCode);
            }

            var movieResults = await _api.GetMovieGenresAsync();
            if (movieResults.IsSuccess)
            {
                Console.WriteLine("Found {0} movies", movieResults.Data.Count());
                foreach (var genre in movieResults.Data)
                {
                    _movieRepository.InsertOrUpdate(genre);
                }
            }
            else
            {
                Console.WriteLine("Error occurred retrieving movie genres: " + showResults.StatusCode);
            }
        }

        internal async Task UpdateMoviesAsync()
        {
            const int startId = 0;
            const int endId = int.MaxValue;
            for (var id = startId; id < endId; id++)
            {
                await UpdateMovieAsync(id);
            }
        }

        private async Task UpdateMovieAsync(int id)
        {
            // get keywords
            // get images
            // get movie
            // combine

            Movie movie;
            var movieResult = await _api.GetMovieAsync(id);
            if (movieResult.IsSuccess)
            {
                movie = movieResult.Data;
            }
            else
            {
                Console.WriteLine("Could not find movie with ID {0}", id);
                return;
            }

            var keywordResult = await _api.GetMovieKeywordsAsync(id);
            if (keywordResult.IsSuccess)
            {
                movie.Keywords.AddRange(keywordResult.Data);
            }

            var imageResult = await _api.GetMovieImagesAsync(id);
            if (imageResult.IsSuccess)
            {
                movie.Posters.AddRange(imageResult.Data.Posters);
                movie.Backdrops.AddRange(imageResult.Data.Backdrops);
            }

            _movieRepository.InsertOrUpdate(movie);
        }
    }
}