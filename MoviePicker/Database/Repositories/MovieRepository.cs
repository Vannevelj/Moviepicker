using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
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
            Console.WriteLine("Inserting genre \"{0}\" with TMDb ID {1}", genre.Name, genre.TmdbId);
            _context.Genres.AddOrUpdate(genre);
            _context.SaveChanges();
        }

        public void InsertOrUpdate(Movie movie)
        {
            var localMovie = _context.Movies.SingleOrDefault(x => x.TmdbId == movie.TmdbId);
            if (localMovie == null)
            {
                Console.WriteLine("Inserting movie \"{0}\" with TMDb ID {1}", movie.Title, movie.TmdbId);
                movie.AddedOn = DateTime.UtcNow;
                movie.LastUpdatedOn = DateTime.UtcNow;
                _context.Movies.Add(movie);
            }
            else
            {
                Console.WriteLine("Updating movie \"{0}\" with TMDb ID {1}", movie.Title, movie.TmdbId);
                movie.LastUpdatedOn = DateTime.UtcNow;
                localMovie.Update(movie);
            }

            _context.SaveChanges();
        }

        public void InsertOrUpdate(Show show)
        {
            show.AddedOn = DateTime.UtcNow;
            Console.WriteLine("Inserting show \"{0}\" with TMDb ID {1}", show.Name, show.TmdbId);
            _context.Shows.AddOrUpdate(show);
            _context.Genres.AddOrUpdate(show.Genres.ToArray());
            _context.Languages.AddOrUpdate(show.Languages.ToArray());
            _context.Backdrops.AddOrUpdate(show.Backdrops.ToArray());
            _context.Posters.AddOrUpdate(show.Posters.ToArray());
            _context.SaveChanges();
        }

        private void AdjustRating(int userId, int movieId, int change)
        {
            throw new NotImplementedException();
        }
    }
}