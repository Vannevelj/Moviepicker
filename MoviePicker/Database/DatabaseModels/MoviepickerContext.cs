using System.Data.Entity;
using Models.Movies;
using Models.Users;

namespace Database.DatabaseModels
{
    public class MoviepickerContext : DbContext
    {
        public MoviepickerContext() : base("mpdevcontext") { }

        public DbSet<User> Users { get; set; }

        public DbSet<Movie> Movies { get; set; }

        public DbSet<Genre> Genres { get; set; } 

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}