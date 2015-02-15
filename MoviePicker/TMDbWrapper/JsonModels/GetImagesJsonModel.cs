using System.Collections.Generic;
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