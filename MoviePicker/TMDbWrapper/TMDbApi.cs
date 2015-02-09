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

        public async Task<IEnumerable<Genre>> GetshowGenresAsync()
        {
            return (await new GetRequest<GetGenresJsonModel>().ExecuteRequestAsync(GetUrl("genre/tv/list"))).Data.Genres;
        }

        public async Task<IEnumerable<Genre>> GetMovieGenresAsync()
        {
            return (await new GetRequest<GetGenresJsonModel>().ExecuteRequestAsync(GetUrl("genre/movie/list"))).Data.Genres;
        }

        public async Task<Movie> GetMovieAsync(int movieId)
        {
            return (await new GetRequest<Movie>().ExecuteRequestAsync(GetUrl("movie/" + movieId))).Data;
        }

        public async Task<Show> GetShowAsync(int showId)
        {
            return (await new GetRequest<Show>().ExecuteRequestAsync(GetUrl("tv/" + showId))).Data;
        }

        public async Task<ChangeResponse> GetChangedMoviesAsync(DateTime from, DateTime to, int page)
        {
            return (await GetChangesAsync(GetUrl("movie/changes"), from, to, page)).Data;
        }

        public async Task<ChangeResponse> GetChangedShowAsync(DateTime from, DateTime to, int page)
        {
            return (await GetChangesAsync(GetUrl("tv/changes"), from, to, page)).Data;
        }

        private async Task<Response<ChangeResponse>> GetChangesAsync(string url, DateTime from, DateTime to, int page)
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

        public async Task<IEnumerable<Keyword>> GetMovieKeywordsAsync(int movieId)
        {
            return (await GetKeywordsAsync(GetUrl("movie/" + movieId + "/keywords"))).Data.Keywords;
        }

        public async Task<IEnumerable<Keyword>> GetShowKeywordsAsync(int showId)
        {
            return (await GetKeywordsAsync(GetUrl("tv/" + showId + "/keywords"))).Data.Keywords;
        }

        private async Task<Response<GetKeywordsJsonModel>> GetKeywordsAsync(string url)
        {
            return await new GetRequest<GetKeywordsJsonModel>().ExecuteRequestAsync(url);
        }

        public async Task<GetImagesJsonModel> GetMovieImagesAsync(int movieId)
        {
            return (await GetImagesAsync(GetUrl("movie/" + movieId + "/images"))).Data;
        }

        public async Task<GetImagesJsonModel> GetShowImagesAsyncAsync(int showId)
        {
            return (await GetImagesAsync(GetUrl("tv/" + showId + "/images"))).Data;
        }

        private async Task<Response<GetImagesJsonModel>> GetImagesAsync(string url)
        {
            return await new GetRequest<GetImagesJsonModel>().ExecuteRequestAsync(url);
        }

    }
}