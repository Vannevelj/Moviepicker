using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using Models.Movies;
using Models.Users;

namespace Database.DatabaseModels
{
    public class MoviepickerContext : DbContext
    {
        public MoviepickerContext() : base("mpdevcontext")
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Movie> Movies { get; set; }

        public DbSet<Genre> Genres { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<User>().HasKey(x => x.Email);
            modelBuilder.Entity<User>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Movie>().ToTable("Movies");
            modelBuilder.Entity<Movie>().HasKey(x => x.TmdbId);
            modelBuilder.Entity<Movie>().HasMany(x => x.Genres).WithMany().Map(x =>
            {
                x.ToTable("MovieGenres");
                x.MapLeftKey("MovieId");
                x.MapRightKey("GenreId");
            });
            modelBuilder.Entity<Movie>().HasMany(x => x.SpokenLanguages).WithMany().Map(x =>
            {
                x.ToTable("MovieLanguages");
                x.MapLeftKey("MovieId");
                x.MapRightKey("LanguageCode");
            });

            modelBuilder.Entity<Genre>().ToTable("Genres");
            modelBuilder.Entity<Genre>().HasKey(x => x.Id);

            modelBuilder.Entity<Language>().ToTable("Languages");
            modelBuilder.Entity<Language>().HasKey(x => x.Iso);
        }
    }
}