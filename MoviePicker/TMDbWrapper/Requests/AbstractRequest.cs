using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TMDbWrapper.Requests
{
    public abstract class AbstractRequest<TResponse> where TResponse : class
    {
        public async Task<Response<TResponse>> ExecuteRequestAsync(string url, AuthenticationHeaderValue authHeader = null, HttpContent httpContent = null)
        {
            return await InternalExecuteRequestAsync(url, authHeader, httpContent);
        }

        protected async Task<Response<TResponse>> InternalExecuteRequestAsync(string url, AuthenticationHeaderValue authenticationHeader = null, HttpContent httpContent = null)
        {
            var client = CreateClient(authenticationHeader);
            var response = await ExecuteRequestSpecificBehaviourAsync(client, url, httpContent);
            client.Dispose();
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<TResponse>(json);
                return new Response<TResponse>
                {
                    Data = data,
                    StatusCode = response.StatusCode
                };
            }

            return new Response<TResponse>
            {
                Data = null,
                StatusCode = response.StatusCode
            };
        }

        private HttpClient CreateClient(AuthenticationHeaderValue authenticationHeader = null)
        {
            var client = new HttpClient();
            if (authenticationHeader != null)
            {
                client.DefaultRequestHeaders.Authorization = authenticationHeader;
            }
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        protected abstract Task<HttpResponseMessage> ExecuteRequestSpecificBehaviourAsync(HttpClient client, string url, HttpContent httpContent);
    }
}
