using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Models.Movies
{
    public class Language
    {
        [JsonProperty("iso_639_1")]
        public string Iso { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
