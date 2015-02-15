using System.Net;

namespace TMDbWrapper.Requests
{
    public class Response<TResponse>
    {
        public TResponse Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
    }
}