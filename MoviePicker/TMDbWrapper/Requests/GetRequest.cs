using System.Net.Http;
using System.Threading.Tasks;

namespace TMDbWrapper.Requests
{
    public class GetRequest<TResponse> : AbstractRequest<TResponse> where TResponse : class
    {
        protected override async Task<HttpResponseMessage> ExecuteRequestSpecificBehaviourAsync(HttpClient client,
                                                                                                string url, HttpContent httpContent)
        {
            return await client.GetAsync(url);
        }
    }
}