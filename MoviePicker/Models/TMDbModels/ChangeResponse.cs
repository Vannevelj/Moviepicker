using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Models.TMDbModels
{
    public class ChangeResponse
    {
        [JsonProperty("results")]
        public IEnumerable<Change> Changes { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }

        [JsonProperty("total_results")]
        public int TotalResults { get; set; }
    }
}
