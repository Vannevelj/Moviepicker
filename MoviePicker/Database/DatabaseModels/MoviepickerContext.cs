using System.Data.Entity;
using Models.Users;

namespace Database.DatabaseModels
{
    public class MoviepickerContext : DbContext
    {
        public MoviepickerContext() : base("mpdevcontext") { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}