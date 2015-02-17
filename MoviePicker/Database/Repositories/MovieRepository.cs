using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Database.DatabaseModels;
using Database.Repositories.Declarations;
using Models.Movies;

namespace Database.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MoviepickerContext _context;

        public MovieRepository(MoviepickerContext context)
        {
            _context = context;
        }

        public void LikeMovie(int userId, int movieId)
        {
            // retrieve movie from database
            // increment genres, actors, language and year
            // write to database
            // indicate movie has been rated by this user
            AdjustRating(userId, movieId, +1);
        }

        public void DislikeMovie(int userId, int movieId)
        {
            // retrieve movie from database
            // decrement genres, actors, language and year
            // write to database
            // indicate movie has been rated by this user
            AdjustRating(userId, movieId, -1);
        }

        public IEnumerable<Movie> GetMovies(int userId)
        {
            // get all preferences from user
            // order preferences by rating
            // search for movie constrained by the first preference
            // search for movie constrained by the first and second preferences
            // search for movie constrained by (year - 5), (year + 5) and first genre
            // For each movie that isn't inside the database: add it

            throw new NotImplementedException();
        }

        public void InsertOrUpdate(Genre genre)
        {
            if (!_context.Genres.Any(x => x.TMDbId == genre.TMDbId))
            {
                Console.WriteLine("Inserting genre \"{0}\" with TMDb ID {1}", genre.Name, genre.TMDbId);
                _context.Genres.Add(genre);
                _context.SaveChanges();
            }
        }

        public void InsertOrUpdate(Movie movie)
        {
            if (!_context.Movies.Any(x => x.TmdbId == movie.TmdbId))
            {
                var localLanguages = _context.Languages.ToList();
                foreach (var language in movie.SpokenLanguages)
                {
                    // More info about this approach: http://stackoverflow.com/q/28545146/1864167
                    var localLanguage = localLanguages.Find(x => x.Iso == language.Iso);

                    if (localLanguage != null)
                    {
                        language.Id = localLanguage.Id;
                        _context.Entry(localLanguage).State = EntityState.Detached;
                        _context.Languages.Attach(language);
                    }
                }

                movie.AddedOn = DateTime.UtcNow;
                Console.WriteLine("Inserting movie \"{0}\" with TMDb ID {1}", movie.Title, movie.TmdbId);
                _context.Movies.Add(movie);
                _context.SaveChanges();
            }
        }

        private void AdjustRating(int userId, int movieId, int change)
        {
            throw new NotImplementedException();
        }
    }
}