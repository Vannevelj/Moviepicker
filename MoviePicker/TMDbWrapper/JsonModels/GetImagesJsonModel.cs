using System.Collections.Generic;
using Models.Movies;
using Newtonsoft.Json;

namespace TMDbWrapper.JsonModels
{
    public class GetImagesJsonModel
    {
        [JsonProperty("backdrops")]
        public IEnumerable<BackdropImageInfo> Backdrops { get; set; }

        [JsonProperty("posters")]
        public IEnumerable<PosterImageInfo> Posters { get; set; }
    }
}