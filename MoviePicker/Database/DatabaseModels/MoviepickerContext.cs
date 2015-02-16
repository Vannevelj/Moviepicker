﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Data.Entity;
using Models.Movies;
using Models.Users;

namespace Database.DatabaseModels
{
    public class MoviepickerContext : DbContext
    {
        public MoviepickerContext() : base("name=mpdevcontext")
        {
        }

        // Needed for Effort to mock the database
        public MoviepickerContext(DbConnection connection) : base(connection, true)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public IDbSet<Language> Languages { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<User>().HasKey(x => x.Email);
            modelBuilder.Entity<User>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Movie>().ToTable("Movies");
            modelBuilder.Entity<Movie>().HasKey(x => x.Id);
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
            modelBuilder.Entity<Movie>().HasMany(x => x.Keywords).WithMany().Map(x =>
            {
                x.ToTable("MovieKeywords");
                x.MapLeftKey("MovieId");
                x.MapRightKey("KeywordId");
            });
            modelBuilder.Entity<Movie>().HasMany(x => x.Backdrops).WithMany().Map(x =>
            {
                x.ToTable("MovieBackdrops");
                x.MapLeftKey("MovieId");
                x.MapRightKey("ImageInfoId");
            });
            modelBuilder.Entity<Movie>().HasMany(x => x.Posters).WithMany().Map(x =>
            {
                x.ToTable("MoviePosters");
                x.MapLeftKey("MovieId");
                x.MapRightKey("ImageInfoId");
            });

            modelBuilder.Entity<Genre>().ToTable("Genres");
            modelBuilder.Entity<Genre>().HasKey(x => x.Id);
            modelBuilder.Entity<Genre>().Property(x => x.TMDbId).IsRequired();
            modelBuilder.Entity<Genre>().Property(x => x.Name).IsRequired();
            modelBuilder.Entity<Genre>()
                .Property(x => x.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Language>().ToTable("Languages");
            modelBuilder.Entity<Language>().HasKey(x => x.Id);

            modelBuilder.Entity<ImageInfo>().ToTable("Images");
            modelBuilder.Entity<ImageInfo>().HasKey(x => x.Id);
        }
    }
}