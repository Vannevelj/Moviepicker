using System.Data.Entity.Migrations;

namespace Database.Migrations
{
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Genres",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                    Movie_TmdbId = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Movies", t => t.Movie_TmdbId)
                .Index(t => t.Movie_TmdbId);

            CreateTable(
                "dbo.Movies",
                c => new
                {
                    TmdbId = c.Int(nullable: false, identity: true),
                    ImdbId = c.String(),
                    Title = c.String(),
                    OriginalTitle = c.String(),
                    Status = c.String(),
                    Tagline = c.String(),
                    Overview = c.String(),
                    BackdropPath = c.String(),
                    PosterPath = c.String(),
                    Adult = c.Boolean(),
                    ReleaseDate = c.DateTime(),
                    Revenue = c.Long(),
                    Budget = c.Long(),
                    Runtime = c.Int(),
                    Popularity = c.Double(),
                    VoteAverage = c.Double(),
                    VoteCount = c.Int(),
                })
                .PrimaryKey(t => t.TmdbId);

            CreateTable(
                "dbo.Languages",
                c => new
                {
                    Iso = c.String(nullable: false, maxLength: 128),
                    Name = c.String(),
                    Movie_TmdbId = c.Int(),
                })
                .PrimaryKey(t => t.Iso)
                .ForeignKey("dbo.Movies", t => t.Movie_TmdbId)
                .Index(t => t.Movie_TmdbId);

            CreateTable(
                "dbo.Users",
                c => new
                {
                    Email = c.String(nullable: false, maxLength: 128),
                    Id = c.Int(nullable: false, identity: true),
                    Firstname = c.String(),
                    Lastname = c.String(),
                    Password = c.String(),
                })
                .PrimaryKey(t => t.Email);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Languages", "Movie_TmdbId", "dbo.Movies");
            DropForeignKey("dbo.Genres", "Movie_TmdbId", "dbo.Movies");
            DropIndex("dbo.Languages", new[] {"Movie_TmdbId"});
            DropIndex("dbo.Genres", new[] {"Movie_TmdbId"});
            DropTable("dbo.Users");
            DropTable("dbo.Languages");
            DropTable("dbo.Movies");
            DropTable("dbo.Genres");
        }
    }
}