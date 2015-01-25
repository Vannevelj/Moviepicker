using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
