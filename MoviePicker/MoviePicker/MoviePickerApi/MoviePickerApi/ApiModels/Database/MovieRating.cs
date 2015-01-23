using System;

namespace MoviePickerApi.ApiModels.Database
{
    public class MovieRating
    {
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public bool Liked { get; set; }
        public DateTime RatedOn { get; set; }
    }
}