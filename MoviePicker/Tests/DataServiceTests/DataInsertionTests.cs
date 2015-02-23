using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
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
        private Mock<TMDbApi> _api;
        private MoviepickerContext _context;
        private MovieRepository _movieRepository;
        private DataScraper _dataScraper;

        [TestInitialize]
        public void Initialize()
        {
            _context = new MoviepickerContext("name=localdb");
            _movieRepository = new MovieRepository(_context);
            _api = new Mock<TMDbApi>();
            _dataScraper = new DataScraper(_api.Object, _movieRepository);
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
            SetupMethod(_api, x => x.GetMovieGenresAsync(), newMovieGenres);

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
            SetupMethod(_api, x => x.GetShowGenresAsync(), newShowGenres);

            // Act
            await _dataScraper.UpdateGenresAsync();

            // Assert
            _context.Genres.Should().NotBeEmpty();
            _context.Genres.Should().HaveCount(existingGenres.Count() + newMovieGenres.Count() + newShowGenres.Count()); 
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