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

        public void InsertOrUpdate(Genre genre)
        {
            _context.Genres.AddOrUpdate(genre);
            _context.SaveChanges();
        }

        public IEnumerable<Genre> InsertOrUpdate(IEnumerable<Genre> genres)
        {
            foreach (var genre in genres)
            {
                var existingGenre = _context.Genres.SingleOrDefault(x => x.TmdbId == genre.TmdbId);
                if (existingGenre != null)
                {
                    existingGenre.Update(genre);
                    yield return existingGenre;
                }
                else
                {
                    _context.Genres.Add(genre);
                    yield return genre;
                }
            }
            _context.SaveChanges();
        }

        public IEnumerable<Language> InsertOrUpdate(IEnumerable<Language> languages)
        {
            foreach (var language in languages)
            {
                var existingLanguage = _context.Languages.SingleOrDefault(x => x.Iso == language.Iso);
                if (existingLanguage != null)
                {
                    existingLanguage.Update(language);
                    yield return existingLanguage;
                }
                else
                {
                    _context.Languages.Add(language);
                    yield return language;
                }
            }
            _context.SaveChanges();
        }

        public IEnumerable<Keyword> InsertOrUpdate(IEnumerable<Keyword> keywords)
        {
            foreach (var keyword in keywords)
            {
                var existingKeyword = _context.Keywords.SingleOrDefault(x => x.Id == keyword.Id);
                if (existingKeyword != null)
                {
                    existingKeyword.Update(keyword);
                    yield return existingKeyword;
                }
                else
                {
                    _context.Keywords.Add(keyword);
                    yield return keyword;
                }
            }
            _context.SaveChanges();
        }

        public IEnumerable<BackdropImageInfo> InsertOrUpdate(IEnumerable<BackdropImageInfo> backdrops)
        {
            foreach (var backdrop in backdrops)
            {
                var existingBackdrop = _context.Backdrops.SingleOrDefault(x => x.Id == backdrop.Id);
                if (existingBackdrop != null)
                {
                    existingBackdrop.Update(backdrop);
                    yield return existingBackdrop;
                }
                else
                {
                    _context.Backdrops.Add(backdrop);
                    yield return backdrop;
                }
            }
            _context.SaveChanges();
        }

        public IEnumerable<PosterImageInfo> InsertOrUpdate(IEnumerable<PosterImageInfo> posters)
        {
            foreach (var poster in posters)
            {
                var existingPoster = _context.Posters.SingleOrDefault(x => x.Id == poster.Id);
                if (existingPoster != null)
                {
                    existingPoster.Update(poster);
                    yield return existingPoster;
                }
                else
                {
                    _context.Posters.Add(poster);
                    yield return poster;
                }
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
            show.Genres = new List<Genre>(InsertOrUpdate(show.Genres));
            show.Languages = new List<Language>(InsertOrUpdate(show.Languages));
            show.Backdrops = new List<BackdropImageInfo>(InsertOrUpdate(show.Backdrops));
            show.Posters = new List<PosterImageInfo>(InsertOrUpdate(show.Posters));

            var localShow = _context.Shows.SingleOrDefault(x => x.TmdbId == show.TmdbId);
            if (localShow == null)
            {
                Console.WriteLine("Inserting show \"{0}\" with TMDb ID {1}", show.Name, show.TmdbId);
                show.AddedOn = DateTime.UtcNow;
                show.LastUpdatedOn = DateTime.UtcNow;
                _context.Shows.Add(show);
            }
            else
            {
                Console.WriteLine("Updating show \"{0}\" with TMDb ID {1}", show.Name, show.TmdbId);
                show.LastUpdatedOn = DateTime.UtcNow;
                localShow.Update(show);
            }
            _context.SaveChanges();
        }

        private void AdjustRating(int userId, int movieId, int change)
        {
            throw new NotImplementedException();
        }
    }
}