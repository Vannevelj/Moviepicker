#define DATABASE
#undef DATABASE

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

#if DATABASE
using System.Diagnostics;
#endif

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
#if DATABASE
            _context = new MoviepickerContext();
            _context.Database.Log = data => Debug.WriteLine(data);   
#else
            _context = new MoviepickerContext(DbConnectionFactory.CreateTransient());
#endif
            _movieRepository = new MovieRepository(_context);
            _api = new Mock<TMDbApi>();
            _dataScraper = new DataScraper(_api.Object, _movieRepository);
        }

#if DATABASE
        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.Delete();
            _context.SaveChanges();
        }
#endif

        [TestMethod]
        [TestCategory("Unit_DATASCRAPER")]
        public async Task UpdateGenresAsync_WithNewMovieGenres_InsertsGenresInDatabase()
        {
            // Arrange
            var existingGenres = TestDataProvider.GetGenres().ToList();
            _context.Genres.AddRange(existingGenres);
            _context.SaveChanges();

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
            var existingGenres = TestDataProvider.GetGenres().ToList();
            _context.Genres.AddRange(existingGenres);
            _context.SaveChanges();

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
        public async Task UpdateGenresAsync_WithExistingGenres_ChangesExistingGenres()
        {
            // Arrange
            var existingGenres = TestDataProvider.GetGenres().ToList();
            var genreId = existingGenres.First().TmdbId;
            _context.Genres.AddRange(existingGenres);
            _context.SaveChanges();
            var initialId = _context.Genres.Single(x => x.TmdbId == genreId).TmdbId;

            var newShowGenres = new[] {new Genre(genreId, "Cartoon")};
            SetupMethod(_api, x => x.GetShowGenresAsync(), newShowGenres);
            SetupMethod(_api, x => x.GetMovieGenresAsync(), new Genre[] {});

            // Act
            await _dataScraper.UpdateGenresAsync();

            // Assert
            var resultingId = _context.Genres.Single(x => x.TmdbId == genreId).TmdbId;
            _context.Genres.Should().HaveCount(3);
            _context.Genres.Should().BeEquivalentTo(existingGenres.Except(newShowGenres).Union(newShowGenres));
            resultingId.Should().Be(initialId);
        }

        [TestMethod]
        [TestCategory("Unit_DATASCRAPER")]
        public async Task UpdateMovieAsync_WithoutExistingMovie_InsertsMovieInDatabase()
        {
            // Arrange
            const int movieId = 15;
            var newKeywords = TestDataProvider.GetKeywords().ToList();
            var newBackdrops = TestDataProvider.GetBackdrops().ToList();
            var newPosters = TestDataProvider.GetPosters().ToList();
            SetupMethod(_api, x => x.GetMovieKeywordsAsync(movieId), newKeywords);
            SetupMethod(_api, x => x.GetMovieImagesAsync(movieId), new GetImagesJsonModel {Backdrops = newBackdrops, Posters = newPosters});

            var newMovie = TestDataProvider.GetMovie();
            newMovie.Languages = TestDataProvider.GetLanguages().ToList();
            SetupMethod(_api, x => x.GetMovieAsync(movieId), newMovie);

            // Act
            await _dataScraper.UpdateMovieAsync(movieId);

            // Assert
            _context.Movies.Should().HaveCount(1);
            _context.Movies.First().Title.Should().Be(newMovie.Title);
            _context.Keywords.Should().HaveCount(newKeywords.Count);
            _context.Backdrops.Should().HaveCount(newBackdrops.Count);
            _context.Posters.Should().HaveCount(newPosters.Count);
            _context.Languages.Should().HaveCount(newMovie.Languages.Count);
            _context.Genres.Should().HaveCount(newMovie.Genres.Count);
        }

        [TestMethod]
        [TestCategory("Unit_DATASCRAPER")]
        public async Task UpdateMovieAsync_WithExistingKeywords_DoesNotDuplicateKeywords()
        {
            // Arrange
            var existingKeywords = TestDataProvider.GetKeywords().ToList();
            _context.Keywords.AddRange(existingKeywords);
            _context.SaveChanges();

            const int movieId = 16;
            var newKeywords = TestDataProvider.GetKeywords().ToList();
            SetupMethod(_api, x => x.GetMovieKeywordsAsync(movieId), newKeywords);
            SetupMethod(_api, x => x.GetMovieImagesAsync(movieId), new GetImagesJsonModel {Backdrops = Enumerable.Empty<BackdropImageInfo>(), Posters = Enumerable.Empty<PosterImageInfo>()});

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

            const int movieId = 17;
            SetupMethod(_api, x => x.GetMovieKeywordsAsync(movieId), Enumerable.Empty<Keyword>());
            SetupMethod(_api, x => x.GetMovieImagesAsync(movieId), new GetImagesJsonModel {Backdrops = Enumerable.Empty<BackdropImageInfo>(), Posters = Enumerable.Empty<PosterImageInfo>()});

            var newMovie = TestDataProvider.GetMovie();
            var newLanguages = TestDataProvider.GetLanguages().ToList();
            newMovie.Languages.AddRange(newLanguages);
            SetupMethod(_api, x => x.GetMovieAsync(movieId), newMovie);

            // Act
            await _dataScraper.UpdateMovieAsync(movieId);

            // Assert
            _context.Languages.Should().HaveCount(existingLanguages.Count);
            _context.Movies.First().Languages.Should().HaveCount(newLanguages.Count);
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

            const int movieId = 18;
            SetupMethod(_api, x => x.GetMovieKeywordsAsync(movieId), Enumerable.Empty<Keyword>());
            SetupMethod(_api, x => x.GetMovieImagesAsync(movieId), new GetImagesJsonModel {Backdrops = Enumerable.Empty<BackdropImageInfo>(), Posters = Enumerable.Empty<PosterImageInfo>()});

            var newMovie = TestDataProvider.GetMovie();
            var newGenres = TestDataProvider.GetGenres().ToList();
            newMovie.Genres.AddRange(newGenres);
            SetupMethod(_api, x => x.GetMovieAsync(movieId), newMovie);

            // Act
            await _dataScraper.UpdateMovieAsync(movieId);

            // Assert
            _context.Genres.Should().HaveCount(existingGenres.Count);
            _context.Movies.First().Genres.Should().HaveCount(newGenres.Count);
            _context.Genres.Should().BeEquivalentTo(newGenres);
        }

        [TestMethod]
        [TestCategory("Unit_DATASCRAPER")]
        public async Task UpdateMovieAsync_WithExistingMovie_UpdatesFields()
        {
            // Arrange
            const int movieId = 19;
            var newKeywords = TestDataProvider.GetKeywords().ToList();
            var newBackdrops = TestDataProvider.GetBackdrops().ToList();
            var newPosters = TestDataProvider.GetPosters().ToList();
            SetupMethod(_api, x => x.GetMovieKeywordsAsync(movieId), newKeywords);
            SetupMethod(_api, x => x.GetMovieImagesAsync(movieId), new GetImagesJsonModel {Backdrops = newBackdrops, Posters = newPosters});

            var existingMovie = TestDataProvider.GetMovie();
            _context.Movies.Add(existingMovie);
            _context.SaveChanges();

            var newMovie = TestDataProvider.GetMovie();
            const string newTitle = "New Title";
            newMovie.Title = newTitle;
            SetupMethod(_api, x => x.GetMovieAsync(movieId), newMovie);

            // Act
            await _dataScraper.UpdateMovieAsync(movieId);

            // Assert
            _context.Movies.Should().HaveCount(1);
            _context.Movies.First().Title.Should().Be(newTitle);
            _context.Keywords.Should().HaveCount(newKeywords.Count);
            _context.Backdrops.Should().HaveCount(newBackdrops.Count);
            _context.Posters.Should().HaveCount(newPosters.Count);
            _context.Languages.Should().HaveCount(newMovie.Languages.Count);
        }

        [TestMethod]
        [TestCategory("Unit_DATASCRAPER")]
        public async Task UpdateShowAsync_WithoutExistingShow_InsertsShowInDatabase()
        {
            // Arrange
            const int showId = 20;
            var newKeywords = TestDataProvider.GetKeywords().ToList();
            var newBackdrops = TestDataProvider.GetBackdrops().ToList();
            var newPosters = TestDataProvider.GetPosters().ToList();
            SetupMethod(_api, x => x.GetShowKeywordsAsync(showId), newKeywords);
            SetupMethod(_api, x => x.GetShowImagesAsync(showId), new GetImagesJsonModel {Backdrops = newBackdrops, Posters = newPosters});

            var newShow = TestDataProvider.GetShow();
            newShow.Languages = TestDataProvider.GetLanguages().ToList();
            SetupMethod(_api, x => x.GetShowAsync(showId), newShow);

            // Act
            await _dataScraper.UpdateShowAsync(showId);

            // Assert
            _context.Shows.Should().HaveCount(1);
            _context.Shows.First().Name.Should().Be(newShow.Name);
            _context.Backdrops.Should().HaveCount(newBackdrops.Count);
            _context.Posters.Should().HaveCount(newPosters.Count);
            _context.Languages.Should().HaveCount(newShow.Languages.Count);
        }

        [TestMethod]
        [TestCategory("Unit_DATASCRAPER")]
        public async Task UpdateShowAsync_WithExistingShow_UpdatesFields()
        {
            // Arrange
            const int showId = 21;
            var newKeywords = TestDataProvider.GetKeywords().ToList();
            var newBackdrops = TestDataProvider.GetBackdrops().ToList();
            var newPosters = TestDataProvider.GetPosters().ToList();
            SetupMethod(_api, x => x.GetShowKeywordsAsync(showId), newKeywords);
            SetupMethod(_api, x => x.GetShowImagesAsync(showId), new GetImagesJsonModel {Backdrops = newBackdrops, Posters = newPosters});

            var newShow = TestDataProvider.GetShow();
            newShow.Languages = TestDataProvider.GetLanguages().ToList();
            SetupMethod(_api, x => x.GetShowAsync(showId), newShow);

            // Act
            await _dataScraper.UpdateShowAsync(showId);

            // Assert
            _context.Shows.Should().HaveCount(1);
            _context.Shows.First().Name.Should().Be(newShow.Name);
            _context.Backdrops.Should().HaveCount(newBackdrops.Count);
            _context.Posters.Should().HaveCount(newPosters.Count);
            _context.Languages.Should().HaveCount(newShow.Languages.Count);
        }

        [TestMethod]
        [TestCategory("Unit_DATASCRAPER")]
        public async Task UpdateShowAsync_WithExistingLanguages_DoesNotDuplicateLanguages()
        {
            // Arrange
            var existingLanguages = TestDataProvider.GetLanguages().ToList();
            _context.Languages.AddRange(existingLanguages);
            _context.SaveChanges();

            const int showId = 22;
            SetupMethod(_api, x => x.GetShowKeywordsAsync(showId), Enumerable.Empty<Keyword>());
            SetupMethod(_api, x => x.GetShowImagesAsync(showId), new GetImagesJsonModel {Backdrops = Enumerable.Empty<BackdropImageInfo>(), Posters = Enumerable.Empty<PosterImageInfo>()});

            var newShow = TestDataProvider.GetShow();
            var newLanguages = TestDataProvider.GetLanguages().ToList();
            newShow.Languages.AddRange(newLanguages);
            SetupMethod(_api, x => x.GetShowAsync(showId), newShow);

            // Act
            await _dataScraper.UpdateShowAsync(showId);

            // Assert
            _context.Languages.Should().HaveCount(existingLanguages.Count);
            _context.Shows.First().Languages.Should().HaveCount(newLanguages.Count);
            _context.Languages.Should().BeEquivalentTo(newLanguages);
        }

        [TestMethod]
        [TestCategory("Unit_DATASCRAPER")]
        public async Task UpdateShowAsync_WithExistingGenres_DoesNotDuplicateGenres()
        {
            // Arrange
            var existingGenres = TestDataProvider.GetGenres().ToList();
            _context.Genres.AddRange(existingGenres);
            _context.SaveChanges();

            const int showId = 23;
            SetupMethod(_api, x => x.GetShowKeywordsAsync(showId), Enumerable.Empty<Keyword>());
            SetupMethod(_api, x => x.GetShowImagesAsync(showId), new GetImagesJsonModel {Backdrops = Enumerable.Empty<BackdropImageInfo>(), Posters = Enumerable.Empty<PosterImageInfo>()});

            var newShow = TestDataProvider.GetShow();
            var newGenres = TestDataProvider.GetGenres().ToList();
            newShow.Genres.AddRange(newGenres);
            SetupMethod(_api, x => x.GetShowAsync(showId), newShow);

            // Act
            await _dataScraper.UpdateShowAsync(showId);

            // Assert
            _context.Genres.Should().HaveCount(existingGenres.Count);
            _context.Shows.First().Genres.Should().HaveCount(newGenres.Count);
            _context.Genres.Should().BeEquivalentTo(newGenres);
        }

        [TestMethod]
        [TestCategory("Unit_DATASCRAPER")]
        public async Task UpdateMovieAsync_WithNewGenres_AttachesNewGenresToMovieObject()
        {
            // Arrange
            var existingGenres = TestDataProvider.GetGenres().ToList();
            _context.Genres.AddRange(existingGenres);
            _context.SaveChanges();

            const int movieId = 24;
            SetupMethod(_api, x => x.GetMovieKeywordsAsync(movieId), Enumerable.Empty<Keyword>());
            SetupMethod(_api, x => x.GetMovieImagesAsync(movieId), new GetImagesJsonModel {Backdrops = Enumerable.Empty<BackdropImageInfo>(), Posters = Enumerable.Empty<PosterImageInfo>()});

            var newMovie = TestDataProvider.GetMovie();
            var newGenres = TestDataProvider.GetGenres().ToList();
            newGenres.Add(new Genre(98412, "TestGenre"));
            newMovie.Genres = newGenres;
            SetupMethod(_api, x => x.GetMovieAsync(movieId), newMovie);

            // Act
            await _dataScraper.UpdateMovieAsync(movieId);

            // Assert
            _context.Movies.Count().Should().Be(1);
            _context.Movies.First().Genres.Should().HaveCount(newGenres.Count);
            _context.Movies.First().Genres.Should().BeEquivalentTo(newGenres);
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