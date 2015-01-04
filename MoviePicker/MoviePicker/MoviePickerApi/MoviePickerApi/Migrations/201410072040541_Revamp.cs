namespace MoviePickerApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Revamp : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GenrePreferences",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        GenreId = c.Int(nullable: false),
                        Rating = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.GenreId });
            
            CreateTable(
                "dbo.Genres",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Movie_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Movies", t => t.Movie_Id)
                .Index(t => t.Movie_Id);
            
            CreateTable(
                "dbo.Movies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImdbId = c.String(),
                        Title = c.String(),
                        OriginalTitle = c.String(),
                        Status = c.String(),
                        Tagline = c.String(),
                        Overview = c.String(),
                        BackdropPath = c.String(),
                        PosterPath = c.String(),
                        Adult = c.Boolean(nullable: false),
                        ReleaseDate = c.DateTime(),
                        Revenue = c.Long(nullable: false),
                        Budget = c.Long(nullable: false),
                        Runtime = c.Int(),
                        Popularity = c.Double(nullable: false),
                        VoteAverage = c.Double(nullable: false),
                        VoteCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MovieRatings",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        MovieId = c.Int(nullable: false),
                        Liked = c.Boolean(nullable: false),
                        RatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.MovieId });
            
            CreateTable(
                "dbo.YearPreferences",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        Rating = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.Year });
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Genres", "Movie_Id", "dbo.Movies");
            DropIndex("dbo.Genres", new[] { "Movie_Id" });
            DropTable("dbo.YearPreferences");
            DropTable("dbo.MovieRatings");
            DropTable("dbo.Movies");
            DropTable("dbo.Genres");
            DropTable("dbo.GenrePreferences");
        }
    }
}
