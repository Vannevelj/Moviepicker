using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Models.TMDbModels
{
    public struct Change
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("adult")]
        public bool? IsAdult { get; set; }
    }
}
