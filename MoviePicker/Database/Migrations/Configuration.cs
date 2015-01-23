using System.Data.Entity.Migrations;

namespace Database.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<DatabaseModels.MoviepickerContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(DatabaseModels.MoviepickerContext context)
        {

        }
    }
}