using System.Data.Entity.Migrations;
using Database.DatabaseModels;

namespace Migrations.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<MoviepickerContext>
    {
        public Configuration()
        {
            AutomaticMigrationDataLossAllowed = true;
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(MoviepickerContext context)
        {
        }
    }
}