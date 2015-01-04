using System.Web.Http;
using TMDbLib.Client;

namespace MoviePickerApi.Controllers
{
    public class BaseController : ApiController
    {
        protected const string ApiKey = "13f441b40fdb830f1f661b50a634304d";
        protected TMDbClient Client = new TMDbClient(ApiKey);
    }
}
