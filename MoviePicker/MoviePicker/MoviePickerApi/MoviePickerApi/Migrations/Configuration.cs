namespace MoviePickerApi.Migrations
{
    using System.Data.Entity.Migrations;
    using Models;
    using Shared.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<MoviePickerContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(MoviePickerContext context)
        {
            context.Users.AddOrUpdate(x => x.Id, new[]
            {
                new User
                {
                    Id = 1,
                    Email = "Dev",
                    Password = "Dev",
                    Firstname = "Jeroen",
                    Lastname = "Dev"
                }
            });
        }
    }
}
