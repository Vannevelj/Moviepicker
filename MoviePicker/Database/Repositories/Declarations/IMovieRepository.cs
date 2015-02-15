using System.Collections.Generic;
using Models.Movies;

namespace Database.Repositories.Declarations
{
    public interface IMovieRepository
    {
        void LikeMovie(int userId, int movieId);
        void DislikeMovie(int userId, int movieId);
        IEnumerable<Movie> GetMovies(int userId);
        void InsertOrUpdate(Genre genre);
    }
}