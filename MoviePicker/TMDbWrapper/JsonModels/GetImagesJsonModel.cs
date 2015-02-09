using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Movies;
using Newtonsoft.Json;

namespace TMDbWrapper.JsonModels
{
    public class GetImagesJsonModel
    {
        [JsonProperty("backdrops")]
        public IEnumerable<ImageInfo> Backdrops { get; set; }

        [JsonProperty("posters")]
        public IEnumerable<ImageInfo> Posters { get; set; } 
    }
}
