using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TMDbWrapper.Requests
{
    public abstract class AbstractRequest<TResponse> where TResponse : class
    {
        public async Task<Response<TResponse>> ExecuteRequestAsync(string url,
                                                                   AuthenticationHeaderValue authHeader = null,
                                                                   HttpContent httpContent = null,
                                                                   Dictionary<string, string> urlParameters = null)
        {
            return await InternalExecuteRequestAsync(url, authHeader, httpContent, urlParameters);
        }

        protected async Task<Response<TResponse>> InternalExecuteRequestAsync(string url,
                                                                              AuthenticationHeaderValue authenticationHeader = null,
                                                                              HttpContent httpContent = null,
                                                                              Dictionary<string, string> urlParameters = null)
        {
            using (var client = CreateClient(authenticationHeader))
            {
                var response =
                    await ExecuteRequestSpecificBehaviourAsync(client, GetUrl(url, urlParameters), httpContent);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<TResponse>(json);
                    return new Response<TResponse>
                    {
                        Data = data,
                        StatusCode = response.StatusCode,
                        IsSuccess = true
                    };
                }

                return new Response<TResponse>
                {
                    Data = null,
                    StatusCode = response.StatusCode,
                    IsSuccess = false
                };
            }
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

        private string GetUrl(string url, Dictionary<string, string> urlParameters)
        {
            var sb = new StringBuilder(url);
            foreach (var parameter in urlParameters ?? new Dictionary<string, string>())
            {
                sb.Append(string.Format("&{0}={1}", parameter.Key, parameter.Value));
            }
            return sb.ToString();
        }

        protected abstract Task<HttpResponseMessage> ExecuteRequestSpecificBehaviourAsync(HttpClient client, string url,
                                                                                          HttpContent httpContent);
    }
}