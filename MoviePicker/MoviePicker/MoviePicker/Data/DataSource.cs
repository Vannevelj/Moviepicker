using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using Shared.Models;

namespace MoviePicker.Data
{
    public class DataSource
    {
        private readonly HttpClient _client;
        private const string BaseUrl = "http://mpdev.azurewebsites.net/api";

        public DataSource()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<IEnumerable<Genre>> GetGenresAsync()
        {
            var data = await _client.GetStringAsync(BaseUrl + "/genres");
            return JsonConvert.DeserializeObject<List<Genre>>(data);
        }

        public async Task<IEnumerable<MovieSearchResult>> GetMoviesForGenreAsync(int genreId)
        {
            var data = await _client.GetStringAsync(BaseUrl + "/genres/" + genreId);
            return JsonConvert.DeserializeObject<List<MovieSearchResult>>(data);
        }

        public async Task<Movie> GetMovieAsync(int movieId)
        {
            var data = await _client.GetStringAsync(BaseUrl + "/movies/" + movieId);
            return JsonConvert.DeserializeObject<Movie>(data); 
        }

        public async Task<HttpResponseMessage> RegisterAsync(string email, string password, string firstname, string lastname)
        {
            var content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
					{
						new KeyValuePair<string, string>("email", email),
						new KeyValuePair<string, string>("password", password),
						new KeyValuePair<string, string>("firstname", firstname),
						new KeyValuePair<string, string>("lastname", lastname),
					});

            return await _client.PostAsync(BaseUrl + "/users/create", content);
        }
    }
}
