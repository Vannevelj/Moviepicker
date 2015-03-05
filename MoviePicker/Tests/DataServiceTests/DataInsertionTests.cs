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
using Models.Utilities;
using Moq;
using Tests.TestUtilities;
using TMDbWrapper;
using TMDbWrapper.JsonModels;
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
        [TestCategory("Unit_DATASCRAPER")]
        public async Task UpdateGenresAsync_WithNewMovieGenres_InsertsGenresInDatabase()
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
            _context.Genres.Should().HaveCount(existingGenres.Count() + newMovieGenres.Count());
        }

        [TestMethod]
        [TestCategory("Unit_DATASCRAPER")]
        public async Task UpdateGenresAsync_WithNewShowGenres_InsertsGenresInDatabase()
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
            _context.Genres.Should().HaveCount(existingGenres.Count() + newShowGenres.Count());
        }

        [TestMethod]
        [TestCategory("Unit_DATASCRAPER")]
        public async Task UpdateGenresAsync_WithExistingGenres_DoesNotChangeExistingGenres()
        {
            // Arrange
            const int genreId = 8942;
            var existingGenres = new[] {new Genre(15, "Horror"), new Genre(genreId, "Comedy")};
            _context.Genres.AddRange(existingGenres);
            _context.SaveChanges();
            var initialId = _context.Genres.Single(x => x.TMDbId == genreId).Id;

            var newShowGenres = new[] {new Genre(9845, "Drama"), new Genre(genreId, "Comedy")};
            SetupMethod(_api, x => x.GetShowGenresAsync(), newShowGenres);
            SetupMethod(_api, x => x.GetMovieGenresAsync(), new Genre[] {});

            // Act
            await _dataScraper.UpdateGenresAsync();

            // Assert
            var resultingId = _context.Genres.Single(x => x.TMDbId == genreId).Id;
            _context.Genres.Should().HaveCount(3);
            resultingId.Should().Be(initialId);
        }

        [TestMethod]
        [TestCategory("Unit_DATASCRAPER")]
        public async Task UpdateMovieAsync_WithoutExistingData_InsertsMovieInDatabase()
        {
            // Arrange
            const int movieId = 15;
            var newKeywords = TestDataProvider.GetKeywords().ToList();
            var newBackdrops = TestDataProvider.GetBackdrops().ToList();
            var newPosters = TestDataProvider.GetPosters().ToList();
            SetupMethod(_api, x => x.GetMovieKeywordsAsync(movieId), newKeywords);
            SetupMethod(_api, x => x.GetMovieImagesAsync(movieId), new GetImagesJsonModel {Backdrops = newBackdrops, Posters = newPosters});

            var newMovie = TestDataProvider.GetMovie();
            newMovie.SpokenLanguages = TestDataProvider.GetLanguages().ToList();
            SetupMethod(_api, x => x.GetMovieAsync(movieId), newMovie);

            // Act
            await _dataScraper.UpdateMovieAsync(movieId);

            // Assert
            _context.Movies.Should().HaveCount(1);
            _context.Movies.First().Title.Should().Be(newMovie.Title);
            _context.Keywords.Should().HaveCount(newKeywords.Count);
            _context.Movies.First().Keywords.Should().HaveCount(newKeywords.Count);
            _context.Movies.First().Backdrops.Should().HaveCount(newBackdrops.Count);
            _context.Movies.First().Posters.Should().HaveCount(newPosters.Count);
            _context.Languages.Should().HaveCount(newMovie.SpokenLanguages.Count);
            _context.Movies.First().SpokenLanguages.Should().HaveCount(newMovie.SpokenLanguages.Count);
        }

        [TestMethod]
        [TestCategory("Unit_DATASCRAPER")]
        public async Task UpdateMovieAsync_WithExistingKeywords_DoesNotDuplicateKeywords()
        {
            // Arrange
            var existingKeywords = TestDataProvider.GetKeywords().ToList();
            _context.Keywords.AddRange(existingKeywords);
            _context.SaveChanges();

            const int movieId = 15;
            var newKeywords = TestDataProvider.GetKeywords().ToList();
            SetupMethod(_api, x => x.GetMovieKeywordsAsync(movieId), newKeywords);
            SetupMethod(_api, x => x.GetMovieImagesAsync(movieId), new GetImagesJsonModel {Backdrops = Enumerable.Empty<ImageInfo>(), Posters = Enumerable.Empty<ImageInfo>()});

            var newMovie = TestDataProvider.GetMovie();
            SetupMethod(_api, x => x.GetMovieAsync(movieId), newMovie);

            // Act
            await _dataScraper.UpdateMovieAsync(movieId);

            // Assert
            _context.Keywords.Should().HaveCount(existingKeywords.Count);
            _context.Movies.First().Keywords.Should().HaveCount(newKeywords.Count);
            _context.Keywords.Should().BeEquivalentTo(newKeywords);
        }

        [TestMethod]
        [TestCategory("Unit_DATASCRAPER")]
        public async Task UpdateMovieAsync_WithExistingLanguages_DoesNotDuplicateLanguages()
        {
            // Arrange
            var existingLanguages = TestDataProvider.GetLanguages().ToList();
            _context.Languages.AddRange(existingLanguages);
            _context.SaveChanges();

            const int movieId = 15;
            SetupMethod(_api, x => x.GetMovieKeywordsAsync(movieId), Enumerable.Empty<Keyword>());
            SetupMethod(_api, x => x.GetMovieImagesAsync(movieId), new GetImagesJsonModel {Backdrops = Enumerable.Empty<ImageInfo>(), Posters = Enumerable.Empty<ImageInfo>()});

            var newMovie = TestDataProvider.GetMovie();
            var newLanguages = TestDataProvider.GetLanguages().ToList();
            newMovie.SpokenLanguages.AddRange(newLanguages);
            SetupMethod(_api, x => x.GetMovieAsync(movieId), newMovie);

            // Act
            await _dataScraper.UpdateMovieAsync(movieId);

            // Assert
            _context.Languages.Should().HaveCount(existingLanguages.Count);
            _context.Movies.First().SpokenLanguages.Should().HaveCount(newLanguages.Count);
            _context.Languages.Should().BeEquivalentTo(newLanguages);
        }

        [TestMethod]
        [TestCategory("Unit_DATASCRAPER")]
        public async Task UpdateMovieAsync_WithExistingGenres_DoesNotDuplicateGenres()
        {
            // Arrange
            var existingGenres = TestDataProvider.GetGenres().ToList();
            _context.Genres.AddRange(existingGenres);
            _context.SaveChanges();

            const int movieId = 15;
            SetupMethod(_api, x => x.GetMovieKeywordsAsync(movieId), Enumerable.Empty<Keyword>());
            SetupMethod(_api, x => x.GetMovieImagesAsync(movieId), new GetImagesJsonModel {Backdrops = Enumerable.Empty<ImageInfo>(), Posters = Enumerable.Empty<ImageInfo>()});

            var newMovie = TestDataProvider.GetMovie();
            var newGenres = TestDataProvider.GetGenres().ToList();
            newMovie.Genres.AddRange(newGenres);
            SetupMethod(_api, x => x.GetMovieAsync(movieId), newMovie);

            // Act
            await _dataScraper.UpdateMovieAsync(movieId);

            // Assert
            _context.Genres.Should().HaveCount(existingGenres.Count);
            _context.Movies.First().SpokenLanguages.Should().HaveCount(newGenres.Count);
            _context.Genres.Should().BeEquivalentTo(newGenres);
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