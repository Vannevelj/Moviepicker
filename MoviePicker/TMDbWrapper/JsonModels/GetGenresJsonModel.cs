using System.Collections.Generic;
using Models.Movies;

namespace TMDbWrapper.JsonModels
{
    public class GetGenresJsonModel
    {
        public IEnumerable<Genre> Genres { get; set; }
    }
}