using System.Collections.Generic;
using Models.Movies;

namespace Database.Repositories.Declarations
{
    public interface IMovieRepository
    {
        void LikeMovie(int userId, int movieId);
        void DislikeMovie(int userId, int movieId);
        IEnumerable<Movie> GetMovies(int userId);
        IEnumerable<Genre> InsertOrUpdate(IEnumerable<Genre> genres);
        IEnumerable<Language> InsertOrUpdate(IEnumerable<Language> languages);
        IEnumerable<Keyword> InsertOrUpdate(IEnumerable<Keyword> keywords);
        IEnumerable<BackdropImageInfo> InsertOrUpdate(IEnumerable<BackdropImageInfo> backdrops);
        IEnumerable<PosterImageInfo> InsertOrUpdate(IEnumerable<PosterImageInfo> posters);
        void InsertOrUpdate(Movie movie);
        void InsertOrUpdate(Show show);
    }
}