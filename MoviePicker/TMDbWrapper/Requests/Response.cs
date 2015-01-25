using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TMDbWrapper.Requests
{
    public class Response<TResponse>
    {
        public TResponse Data { get; set; }

        public HttpStatusCode StatusCode { get; set; }
    }
}
