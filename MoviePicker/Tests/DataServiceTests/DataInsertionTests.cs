using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using Database.DatabaseModels;
using Database.Repositories;
using Effort;
using Effort.Provider;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Movies;
using NSubstitute;
using TMDbWrapper;
using TMDbWrapper.Requests;

namespace Tests.DataServiceTests
{
    [TestClass]
    public class DataInsertionTests
    {
        private TMDbApi _api;
        private MoviepickerContext _context;
        private MovieRepository _movieRepository;
        private DataService.DataService _service;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            EffortProviderConfiguration.RegisterProvider();
        }

        [TestInitialize]
        public void Initialize()
        {          
            var connection = DbConnectionFactory.CreateTransient();
            _context = new MoviepickerContext(connection);
            _movieRepository = new MovieRepository(_context);
            _service = new DataService.DataService(_movieRepository);
            _api = Substitute.For<TMDbApi>();
        }

        [TestMethod]
        public async Task GetGenresAsync_WithNewGenres_InsertsGenresInDatabase()
        {
            // Arrange
            var returnedGenres = Task.Factory.StartNew(() => new Response<IEnumerable<Genre>> { Data = GetGenres() });
            _api.GetMovieGenresAsync().Returns(x => returnedGenres);

            // Act
            await _service.StartScraper();

            // Assert
            _context.Genres.Should().NotBeEmpty();
            _context.Movies.Should().BeEmpty();
        }

        private IEnumerable<Genre> GetGenres()
        {
            return new List<Genre>
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
        }

        private Movie GetMovie()
        {
            return null;
        }
    }
}