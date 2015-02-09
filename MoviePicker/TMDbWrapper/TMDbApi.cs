using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Movies;
using Models.TMDbModels;
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

        public async Task<ChangeResponse> GetChangedMovies(DateTime from, DateTime to, int page)
        {
            return (await GetChanges(GetUrl("movie/changes"), from, to, page)).Data;
        }

        public async Task<ChangeResponse> GetChangedShow(DateTime from, DateTime to, int page)
        {
            return (await GetChanges(GetUrl("tv/changes"), from, to, page)).Data;
        }

        private async Task<Response<ChangeResponse>> GetChanges(string url, DateTime from, DateTime to, int page)
        {
            const string dateFormat = "YYYY-MM-DD";
            var parameters = new Dictionary<string, string>
            {
                {"start_date", from.ToString(dateFormat)},
                {"end_date", to.ToString(dateFormat)},
                {"page", page.ToString()}
            };

            return await new GetRequest<ChangeResponse>().ExecuteRequestAsync(url, urlParameters: parameters); 
        }
    }
}