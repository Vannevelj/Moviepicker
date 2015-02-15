using System.Collections.Generic;
using Models.Movies;
using Newtonsoft.Json;

namespace TMDbWrapper.JsonModels
{
    public class GetKeywordsJsonModel
    {
        [JsonProperty("id")]
        public int MovieId { get; set; }

        [JsonProperty("keywords")]
        public IEnumerable<Keyword> Keywords { get; set; }
    }
}