using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Data.Entity;
using System.Diagnostics;
using Microsoft.AspNet.Identity.EntityFramework;
using Models.Movies;
using Models.Users.Authorization;

namespace Database.DatabaseModels
{
    public class MoviepickerContext : IdentityDbContext<IdentityUser>
    {
        // ReSharper disable once RedundantBaseConstructorCall
        public MoviepickerContext() : base( /*"name=mpdevcontext"*/)
        {
        }

        public MoviepickerContext(DbConnection connection) : base(connection, true)
        {
            Database.Log = msg => Debug.WriteLine(msg);
        }

        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<Show> Shows { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<Keyword> Keywords { get; set; }
        public virtual DbSet<BackdropImageInfo> Backdrops { get; set; }
        public virtual DbSet<PosterImageInfo> Posters { get; set; }
        public virtual DbSet<ClientApplication> ClientApplications { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Movie>().ToTable("Movies");
            modelBuilder.Entity<Movie>().HasKey(x => x.TmdbId);
            modelBuilder.Entity<Movie>().Property(x => x.TmdbId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<Movie>().HasMany(x => x.Genres).WithMany().Map(x =>
            {
                x.ToTable("MovieGenres");
                x.MapLeftKey("MovieId");
                x.MapRightKey("GenreId");
            });
            modelBuilder.Entity<Movie>().HasMany(x => x.Languages).WithMany().Map(x =>
            {
                x.ToTable("MovieLanguages");
                x.MapLeftKey("MovieId");
                x.MapRightKey("LanguageId");
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
                x.MapRightKey("ImageId");
            });
            modelBuilder.Entity<Movie>().HasMany(x => x.Posters).WithMany().Map(x =>
            {
                x.ToTable("MoviePosters");
                x.MapLeftKey("MovieId");
                x.MapRightKey("ImageId");
            });

            modelBuilder.Entity<Genre>().ToTable("Genres");
            modelBuilder.Entity<Genre>().HasKey(x => x.TmdbId);
            modelBuilder.Entity<Genre>().Property(x => x.TmdbId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<Genre>().Property(x => x.TmdbId).IsRequired();
            modelBuilder.Entity<Genre>().Property(x => x.Name).IsRequired();

            modelBuilder.Entity<Language>().ToTable("Languages");
            modelBuilder.Entity<Language>().HasKey(x => x.Iso);
            modelBuilder.Entity<Language>().Property(x => x.Iso).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<Keyword>().ToTable("Keywords");
            modelBuilder.Entity<Keyword>().HasKey(x => x.Id);
            modelBuilder.Entity<Keyword>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<Show>().ToTable("Shows");
            modelBuilder.Entity<Show>().HasKey(x => x.TmdbId);
            modelBuilder.Entity<Show>().Property(x => x.TmdbId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<Show>().HasMany(x => x.Languages).WithMany().Map(x =>
            {
                x.ToTable("ShowLanguages");
                x.MapLeftKey("ShowId");
                x.MapRightKey("LanguageId");
            });
            modelBuilder.Entity<Show>().HasMany(x => x.Genres).WithMany().Map(x =>
            {
                x.ToTable("ShowGenres");
                x.MapLeftKey("ShowId");
                x.MapRightKey("GenreId");
            });
            modelBuilder.Entity<Show>().HasMany(x => x.Backdrops).WithMany().Map(x =>
            {
                x.ToTable("ShowBackdrops");
                x.MapLeftKey("ShowId");
                x.MapRightKey("BackdropId");
            });
            modelBuilder.Entity<Show>().HasMany(x => x.Posters).WithMany().Map(x =>
            {
                x.ToTable("ShowPosters");
                x.MapLeftKey("ShowId");
                x.MapRightKey("PosterId");
            });

            modelBuilder.Entity<BackdropImageInfo>().ToTable("Backdrops");
            modelBuilder.Entity<BackdropImageInfo>().HasKey(x => x.Id);
            modelBuilder.Entity<BackdropImageInfo>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<PosterImageInfo>().ToTable("Posters");
            modelBuilder.Entity<PosterImageInfo>().HasKey(x => x.Id);
            modelBuilder.Entity<PosterImageInfo>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<ClientApplication>().ToTable("ClientApplications");
            modelBuilder.Entity<ClientApplication>().HasKey(x => x.Id);
            modelBuilder.Entity<ClientApplication>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<ClientApplication>().Property(x => x.Name).IsRequired();
            modelBuilder.Entity<ClientApplication>().Property(x => x.Name).HasMaxLength(100);
            modelBuilder.Entity<ClientApplication>().Property(x => x.AllowedOrigin).HasMaxLength(100);

            modelBuilder.Entity<RefreshToken>().ToTable("Refreshtokens");
            modelBuilder.Entity<RefreshToken>().HasKey(x => x.Id);
            modelBuilder.Entity<RefreshToken>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<RefreshToken>().Property(x => x.Subject).IsRequired();
            modelBuilder.Entity<RefreshToken>().Property(x => x.Subject).HasMaxLength(50);
            modelBuilder.Entity<RefreshToken>().Property(x => x.ProtectedTicket).IsRequired();
            modelBuilder.Entity<RefreshToken>().HasRequired(x => x.ClientApplication).WithMany().HasForeignKey(x => x.ClientApplicationId);
        }
    }
}