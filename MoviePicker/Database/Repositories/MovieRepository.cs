using System;
using System.Collections.Generic;
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

        public IEnumerable<Genre> InsertOrUpdate(IEnumerable<Genre> genres)
        {
            foreach (var genre in genres)
            {
                _context.Genres.AddOrUpdate(genre);
                yield return _context.Genres.Single(x => x.TmdbId == genre.TmdbId);
            }
            _context.SaveChanges();
        }

        public IEnumerable<Language> InsertOrUpdate(IEnumerable<Language> languages)
        {
            foreach (var language in languages)
            {
                _context.Languages.AddOrUpdate(language);
                yield return _context.Languages.Single(x => x.Iso == language.Iso);
            }
            _context.SaveChanges();
        }

        public IEnumerable<Keyword> InsertOrUpdate(IEnumerable<Keyword> keywords)
        {
            foreach (var keyword in keywords)
            {
                _context.Keywords.AddOrUpdate(keyword);
                yield return _context.Keywords.Single(x => x.Id == keyword.Id);
            }
            _context.SaveChanges();
        }

        public IEnumerable<BackdropImageInfo> InsertOrUpdate(IEnumerable<BackdropImageInfo> backdrops)
        {
            foreach (var backdrop in backdrops)
            {
                _context.Backdrops.AddOrUpdate(backdrop);
                yield return _context.Backdrops.Single(x => x.Id == backdrop.Id);
            }
            _context.SaveChanges();
        }

        public IEnumerable<PosterImageInfo> InsertOrUpdate(IEnumerable<PosterImageInfo> posters)
        {
            foreach (var poster in posters)
            {
                _context.Posters.AddOrUpdate(poster);
                yield return _context.Posters.Single(x => x.Id == poster.Id);
            }
            _context.SaveChanges();
        }

        public void InsertOrUpdate(Movie movie)
        {
            movie.Genres = new List<Genre>(InsertOrUpdate(movie.Genres));
            movie.Keywords = new List<Keyword>(InsertOrUpdate(movie.Keywords));
            movie.Languages = new List<Language>(InsertOrUpdate(movie.Languages));
            movie.Backdrops = new List<BackdropImageInfo>(InsertOrUpdate(movie.Backdrops));
            movie.Posters = new List<PosterImageInfo>(InsertOrUpdate(movie.Posters));

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

            _context.Genres.AddOrUpdate(show.Genres.ToArray());
            _context.Languages.AddOrUpdate(show.Languages.ToArray());
            _context.Backdrops.AddOrUpdate(show.Backdrops.ToArray());
            _context.Posters.AddOrUpdate(show.Posters.ToArray());
            _context.SaveChanges();
            //_context.Shows.AddOrUpdate(show);
            _context.SaveChanges();
        }

        private void AdjustRating(int userId, int movieId, int change)
        {
            throw new NotImplementedException();
        }
    }
}