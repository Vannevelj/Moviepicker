using System.Collections.Generic;
using Models.Movies;
using Newtonsoft.Json;

namespace TMDbWrapper.JsonModels
{
    public class GetGenresJsonModel
    {
        [JsonProperty("genres")]
        public IEnumerable<Genre> Genres { get; set; }
    }
}