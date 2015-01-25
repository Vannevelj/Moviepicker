using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Movies;

namespace TMDbWrapper.JsonModels
{
    public class GetGenresJsonModel
    {
        public IEnumerable<Genre> Genres { get; set; } 
    }
}
