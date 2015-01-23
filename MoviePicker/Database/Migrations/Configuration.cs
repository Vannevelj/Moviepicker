using System.Data.Entity.Migrations;
using Database.DatabaseModels;
using Models.Users;

namespace Database.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<MoviepickerContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(MoviepickerContext context)
        {
            context.Users.RemoveRange(context.Users);
            
            context.Users.Add(new User
            {
                Email = "test@test.com",
                Firstname = "Frank",
                Lastname = "Bonkers",
                Password = "pissword"
            });
        }
    }
}