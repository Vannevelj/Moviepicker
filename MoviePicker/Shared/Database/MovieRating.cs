using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Database
{
    public class MovieRating
    {
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public bool Liked { get; set; }
        public DateTime RatedOn { get; set; }
    }
}
