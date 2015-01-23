using System.Data.Entity;
using MoviePickerApi.Models.Database;
using MoviePickerApi.Models.Models;

namespace MoviePickerApi.Models
{
    public class MoviePickerContext : DbContext
    {
        public MoviePickerContext() : base("name=MoviePickerContext")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<MovieRating> Ratings { get; set; }
        public DbSet<GenrePreference> GenrePreferences { get; set; }
        public DbSet<YearPreference> YearPreferences { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<MovieRating>().HasKey(x => new {x.UserId, x.MovieId});
            modelBuilder.Entity<GenrePreference>().HasKey(x => new {x.UserId, x.GenreId});
            modelBuilder.Entity<YearPreference>().HasKey(x => new {x.UserId, x.Year});
        }
    }
}