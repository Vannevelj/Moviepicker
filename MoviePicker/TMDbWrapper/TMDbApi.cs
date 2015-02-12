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

        public async Task<Response<IEnumerable<Genre>>> GetShowGenresAsync()
        {
            var response = await new GetRequest<GetGenresJsonModel>().ExecuteRequestAsync(GetUrl("genre/tv/list"));
            return new Response<IEnumerable<Genre>>
                   {
                       Data = response.Data.Genres,
                       StatusCode = response.StatusCode
                   };
        }

        public async Task<Response<IEnumerable<Genre>>> GetMovieGenresAsync()
        {
            var response = await new GetRequest<GetGenresJsonModel>().ExecuteRequestAsync(GetUrl("genre/movie/list"));
            return new Response<IEnumerable<Genre>>
                   {
                       Data = response.Data.Genres,
                       StatusCode = response.StatusCode
                   };
        }

        public async Task<Response<Movie>> GetMovieAsync(int movieId)
        {
            return await new GetRequest<Movie>().ExecuteRequestAsync(GetUrl("movie/" + movieId));
        }

        public async Task<Response<Show>> GetShowAsync(int showId)
        {
            return await new GetRequest<Show>().ExecuteRequestAsync(GetUrl("tv/" + showId));
        }

        public async Task<Response<ChangeResponse>> GetChangedMoviesAsync(DateTime from, DateTime to, int page)
        {
            return await GetChangesAsync(GetUrl("movie/changes"), from, to, page);
        }

        public async Task<Response<ChangeResponse>> GetChangedShowsAsync(DateTime from, DateTime to, int page)
        {
            return await GetChangesAsync(GetUrl("tv/changes"), from, to, page);
        }

        private async Task<Response<ChangeResponse>> GetChangesAsync(string url, DateTime from, DateTime to, int page)
        {
            const string dateFormat = "yyyy-MM-dd";
            var parameters = new Dictionary<string, string>
            {
                {"start_date", from.ToString(dateFormat)},
                {"end_date", to.ToString(dateFormat)},
                {"page", page.ToString()}
            };

            return await new GetRequest<ChangeResponse>().ExecuteRequestAsync(url, urlParameters: parameters); 
        }

        public async Task<Response<IEnumerable<Keyword>>> GetMovieKeywordsAsync(int movieId)
        {
            return await GetKeywordsAsync(GetUrl("movie/" + movieId + "/keywords"));
        }

        public async Task<Response<IEnumerable<Keyword>>> GetShowKeywordsAsync(int showId)
        {
            return await GetKeywordsAsync(GetUrl("tv/" + showId + "/keywords"));
        }

        private async Task<Response<IEnumerable<Keyword>>> GetKeywordsAsync(string url)
        {
            var response = await new GetRequest<GetKeywordsJsonModel>().ExecuteRequestAsync(url);
            return new Response<IEnumerable<Keyword>>()
                   {
                       Data = response.Data.Keywords,
                       StatusCode = response.StatusCode
                   };
        }

        public async Task<Response<GetImagesJsonModel>> GetMovieImagesAsync(int movieId)
        {
            return await GetImagesAsync(GetUrl("movie/" + movieId + "/images"));
        }

        public async Task<Response<GetImagesJsonModel>> GetShowImagesAsync(int showId)
        {
            return await GetImagesAsync(GetUrl("tv/" + showId + "/images"));
        }

        private async Task<Response<GetImagesJsonModel>> GetImagesAsync(string url)
        {
            return await new GetRequest<GetImagesJsonModel>().ExecuteRequestAsync(url);
        }
    }
}