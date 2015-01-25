using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TMDbWrapper.Requests
{
    public class PostRequest<TResponse> : AbstractRequest<TResponse> where TResponse: class 
    {
        protected override async Task<HttpResponseMessage> ExecuteRequestSpecificBehaviourAsync(HttpClient client, string url, HttpContent httpContent)
        {
            return await client.PostAsync(url, httpContent);
        }
    }
}
