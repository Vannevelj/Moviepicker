using Microsoft.AspNet.Identity.EntityFramework;
using Models.Movies;

namespace Models.Preferences
{
    public abstract class Preference
    {
        public IdentityUser User { get; set; }
        public int Score { get; set; }
    }

    public class GenrePreference : Preference
    {
        public Genre Genre { get; set; }
    }
}