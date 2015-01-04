using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Database
{
    public class GenrePreference
    {
        public int UserId { get; set; }
        public int GenreId { get; set; }
        public int Rating { get; set; }
    }

    public class YearPreference
    {
        public int UserId { get; set; }
        public int Year { get; set; }
        public int Rating { get; set; }
    }

    //public class ActorPreference
    //{
    //    public int UserId { get; set; }
    //    public int ActorId { get; set; }
    //    public int Rating { get; set; }
    //}

    //public class LanguagePreference
    //{
    //    public int UserId { get; set; }
    //    public int LanguageId { get; set; }
    //    public int Rating { get; set; }
    //}
}
