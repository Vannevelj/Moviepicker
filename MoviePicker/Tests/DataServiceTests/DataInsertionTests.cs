using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Database.DatabaseModels;
using Database.Repositories;
using DataService;
using Effort;
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
        private Mock<TMDbApi> _api;
        private MoviepickerContext _context;
        private DataScraper _dataScraper;
        private MovieRepository _movieRepository;

        [TestInitialize]
        public void Initialize()
        {
            _context = new MoviepickerContext(DbConnectionFactory.CreateTransient());
            _movieRepository = new MovieRepository(_context);
            _api = new Mock<TMDbApi>();
            _dataScraper = new DataScraper(_api.Object, _movieRepository);
        }

        [TestMethod]
        public async Task GetGenresAsync_WithNewMovieGenres_InsertsGenresInDatabase()
        {
            // Arrange
            var existingGenres = new[] {new Genre(15, "Horror"), new Genre(8942, "Comedy")};
            _context.Genres.AddRange(existingGenres);

            var newMovieGenres = new[] {new Genre(9845, "Drama"), new Genre(87422, "War")};
            SetupMethod(_api, x => x.GetMovieGenresAsync(), newMovieGenres);
            SetupMethod(_api, x => x.GetShowGenresAsync(), new Genre[] {});

            // Act
            await _dataScraper.UpdateGenresAsync();

            // Assert
            _context.Genres.Should().NotBeEmpty();
            _context.Genres.Should().HaveCount(existingGenres.Count() + newMovieGenres.Count());
        }

        [TestMethod]
        public async Task GetGenresAsync_WithNewShowGenres_InsertsGenresInDatabase()
        {
            // Arrange
            var existingGenres = new[] {new Genre(15, "Horror"), new Genre(8942, "Comedy")};
            _context.Genres.AddRange(existingGenres);

            var newShowGenres = new[] {new Genre(9845, "Drama"), new Genre(87422, "War")};
            SetupMethod(_api, x => x.GetShowGenresAsync(), newShowGenres);
            SetupMethod(_api, x => x.GetMovieGenresAsync(), new Genre[] {});

            // Act
            await _dataScraper.UpdateGenresAsync();

            // Assert
            _context.Genres.Should().NotBeEmpty();
            _context.Genres.Should().HaveCount(existingGenres.Count() + newShowGenres.Count());
        }

        [TestMethod]
        public async Task GetGenresAsync_WithExistingGenres_DoesNotChangeExistingGenres()
        {
            // Arrange
            const int genreId = 8942;
            var existingGenres = new[] { new Genre(15, "Horror"), new Genre(genreId, "Comedy") };
            _context.Genres.AddRange(existingGenres);
            _context.SaveChanges();
            int initialId = _context.Genres.Single(x => x.TMDbId == genreId).Id;

            var newShowGenres = new[] { new Genre(9845, "Drama"), new Genre(genreId, "Comedy") };
            SetupMethod(_api, x => x.GetShowGenresAsync(), newShowGenres);
            SetupMethod(_api, x => x.GetMovieGenresAsync(), new Genre[] {});

            // Act
            await _dataScraper.UpdateGenresAsync();

            // Assert
            int resultingId = _context.Genres.Single(x => x.TMDbId == genreId).Id;
            _context.Genres.Should().NotBeEmpty();
            _context.Genres.Should().HaveCount(3);
            resultingId.Should().Be(initialId);
        }

        private void SetupMethod<TType, TResponse>(Mock<TType> api, Expression<Func<TType, Task<Response<TResponse>>>> method, TResponse data) where TType : class
        {
            api.Setup(method)
                .ReturnsAsync(new Response<TResponse>
                {
                    Data = data,
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK
                });
        }
    }
}