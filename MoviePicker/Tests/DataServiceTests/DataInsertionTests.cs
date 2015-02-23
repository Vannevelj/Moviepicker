using System.Collections.Generic;
using System.Configuration;
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
using TMDbWrapper;
using TMDbWrapper.Requests;

namespace Tests.DataServiceTests
{
    [TestClass]
    public class DataInsertionTests
    {
        private TMDbApi _api;
        private MoviepickerContext _context;

        [TestInitialize]
        public void Initialize()
        {
            _context = new MoviepickerContext("name=localdb");
        }

        [TestCleanup]
        public void CleanUp()
        {
            _context.Genres.RemoveRange(_context.Genres);
            _context.Users.RemoveRange(_context.Users);
            _context.Languages.RemoveRange(_context.Languages);
            _context.Movies.RemoveRange(_context.Movies);
            _context.SaveChanges();
        }

        [TestMethod]
        public async Task GetGenresAsync_WithNewGenres_InsertsGenresInDatabase()
        {
            // Arrange
            var existingGenres = new[]
            {
                new Genre
                {
                    TMDbId = 15,
                    Name = "Horror"
                },
                new Genre
                {
                    TMDbId = 8942,
                    Name = "Comedy"
                }
            };
            _context.Genres.AddRange(existingGenres);
            _context.SaveChanges();

            var newMovieGenres = new[]
            {
                new Genre
                {
                    TMDbId = 1234,
                    Name = "Drama"
                },
                new Genre
                {
                    TMDbId = 2345,
                    Name = "War"
                }
            };

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


            var newShowGenres = new[]
            {
                new Genre
                {
                    TMDbId = 3456,
                    Name = "Cartoon"
                },
                new Genre
                {
                    TMDbId = 4567,
                    Name = "History"
                }
            };
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

            var repository = new MovieRepository(_context);
            var dataScraper = new DataScraper(apiMock.Object, repository);

            // Act
            await dataScraper.UpdateGenresAsync();

            // Assert
            _context.Genres.Should().NotBeEmpty();
            _context.Genres.Should().HaveCount(existingGenres.Count() + newMovieGenres.Count() + newShowGenres.Count());
        }
    }
}