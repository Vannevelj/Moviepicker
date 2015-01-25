using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Movies;
using TMDbWrapper.JsonModels;
using TMDbWrapper.Requests;

namespace TMDbWrapper
{
// ReSharper disable once InconsistentNaming
    public class TMDbApi
    {
        private readonly string _apikey;
        private const string BaseUrl = "https://api.themoviedb.org/3/";

        public TMDbApi(string apikey)
        {
            _apikey = apikey;
        }

        private string GetUrl(string apiRequest)
        {
            return BaseUrl + apiRequest + "?api_key=" + _apikey;
        }

        public async Task<IEnumerable<Genre>> GetTvGenres()
        {
            return (await new GetRequest<GetGenresJsonModel>().ExecuteRequestAsync(GetUrl("genre/tv/list"))).Data.Genres;
        }

        public async Task<IEnumerable<Genre>> GetMovieGenres()
        {
            return (await new GetRequest<GetGenresJsonModel>().ExecuteRequestAsync(GetUrl("genre/movie/list"))).Data.Genres;
        }

        public async Task<Movie> GetMovie(int movieId)
        {
            return (await new GetRequest<Movie>().ExecuteRequestAsync(GetUrl("movie/" + movieId))).Data;
        }

        public async Task<IEnumerable<Keyword>> GetKeywords(int movieId)
        {
            return (await new GetRequest<GetKeywordsJsonModel>().ExecuteRequestAsync(GetUrl("movie/" + movieId + "/keywords"))).Data.Keywords;
        }
    }
}