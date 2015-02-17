using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Database.DatabaseModels;
using Database.Repositories;
using DataService;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Movies;
using Moq;
using Tests.Utilities;
using TMDbWrapper;
using TMDbWrapper.Requests;

namespace Tests.DataServiceTests
{
    [TestClass]
    public class DataInsertionTests
    {
        private TMDbApi _api;

        [TestInitialize]
        public void Initialize()
        {
        }

        [TestMethod]
        public async Task GetGenresAsync_WithNewGenres_InsertsGenresInDatabase()
        {
            // Arrange
            var context = new Mock<MoviepickerContext>();
            var genreMock = new Mock<DbSet<Genre>>();
            var existingGenres = TestDataProvider.GetGenres().Take(2).AsQueryable();
            genreMock.As<IQueryable<Genre>>().Setup(x => x.Provider).Returns(existingGenres.Provider);
            genreMock.As<IQueryable<Genre>>().Setup(x => x.Expression).Returns(existingGenres.Expression);
            genreMock.As<IQueryable<Genre>>().Setup(x => x.ElementType).Returns(existingGenres.ElementType);
            genreMock.As<IQueryable<Genre>>().Setup(x => x.GetEnumerator()).Returns(() => existingGenres.GetEnumerator());
            context.Setup(x => x.Genres).Returns(genreMock.Object);

            //var movieMock = new Mock<DbSet<Movie>>();
            //var movies = GetMovies().AsQueryable();
            //movieMock.As<IQueryable<Movie>>().Setup(x => x.Provider).Returns(movies.Provider);
            //movieMock.As<IQueryable<Movie>>().Setup(x => x.Expression).Returns(movies.Expression);
            //movieMock.As<IQueryable<Movie>>().Setup(x => x.ElementType).Returns(movies.ElementType);
            //movieMock.As<IQueryable<Movie>>().Setup(x => x.GetEnumerator()).Returns(movies.GetEnumerator());
            //context.Setup(x => x.Movies).Returns(movieMock.Object);

            var newMovieGenres = TestDataProvider.GetGenres().Skip(2).Take(2);
            var apiMock = new Mock<TMDbApi>();
            apiMock.Setup(x => x.GetMovieGenresAsync())
                .Returns( 
                    Task.Run(
                        () =>
                            new Response<IEnumerable<Genre>>
                            {
                                Data = newMovieGenres,
                                IsSuccess = true,
                                StatusCode = HttpStatusCode.OK
                            }));


            var newShowGenres = TestDataProvider.GetGenres().Skip(4).Take(2);
            apiMock.Setup(x => x.GetShowGenresAsync())
                .Returns( 
                    Task.Run(
                        () =>
                            new Response<IEnumerable<Genre>>
                            {
                                Data = newShowGenres,
                                IsSuccess = true,
                                StatusCode = HttpStatusCode.OK
                            }));

            var repository = new MovieRepository(context.Object);
            var dataScraper = new DataScraper(apiMock.Object, repository);

            // Act
            await dataScraper.UpdateGenresAsync();

            // Assert
            context.Object.Genres.Should().NotBeEmpty();
            context.Object.Genres.Should()
                .HaveCount(existingGenres.Count() + newMovieGenres.Count() + newShowGenres.Count());
            //context.Object.Movies.Should().BeEmpty();
        }
    }
}