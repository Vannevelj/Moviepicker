using System.Data.Entity.Migrations;
using Database.DatabaseModels;

namespace Migrations.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<MoviepickerContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MoviepickerContext context)
        {
        }
    }
}