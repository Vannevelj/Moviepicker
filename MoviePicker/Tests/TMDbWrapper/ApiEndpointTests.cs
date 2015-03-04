using System;
using System.Configuration;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbWrapper;
using TMDbWrapper.Requests;

namespace Tests.TMDbWrapper
{
    [TestClass]
    public class ApiEndpointTests
    {
        private const int ExistingMovieId = 155;
        private const int ExistingShowId = 155;
        private readonly TMDbApi _api = new TMDbApi(ConfigurationManager.AppSettings["apikey"]);

        /// <summary>
        /// The external API limits the amount of requests in a short period
        /// </summary>
        [TestCleanup]
        public void CleanUp()
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));
        }

        [TestMethod]
        public async Task GetShowGenres_ReturnsHttpOk()
        {
            VerifyStatusCode(await _api.GetShowGenresAsync());
        }

        [TestMethod]
        public async Task GetMovieGenres_ReturnsHttpOk()
        {
            VerifyStatusCode(await _api.GetMovieGenresAsync());
        }

        [TestMethod]
        public async Task GetMovie_ReturnsHttpOk()
        {
            VerifyStatusCode(await _api.GetMovieAsync(ExistingMovieId));
        }

        [TestMethod]
        public async Task GetShow_ReturnsHttpOk()
        {
            VerifyStatusCode(await _api.GetShowAsync(ExistingShowId));
        }

        [TestMethod]
        public async Task GetChangedMovies_ReturnsHttpOk()
        {
            var from = new DateTime(2015, 01, 01);
            var to = new DateTime(2015, 01, 02);
            const int page = 1;
            VerifyStatusCode(await _api.GetChangedMoviesAsync(from, to, page));
        }

        [TestMethod]
        public async Task GetChangedShows_ReturnsHttpOk()
        {
            var from = new DateTime(2015, 01, 01);
            var to = new DateTime(2015, 01, 02);
            const int page = 1;
            VerifyStatusCode(await _api.GetChangedShowsAsync(from, to, page));
        }

        [TestMethod]
        public async Task GetMovieKeywords_ReturnsHttpOk()
        {
            VerifyStatusCode(await _api.GetMovieKeywordsAsync(ExistingMovieId));
        }

        [TestMethod]
        public async Task GetShowKeywords_ReturnsHttpOk()
        {
            VerifyStatusCode(await _api.GetShowKeywordsAsync(ExistingShowId));
        }

        [TestMethod]
        public async Task GetMovieImages_ReturnsHttpOk()
        {
            VerifyStatusCode(await _api.GetMovieImagesAsync(ExistingMovieId));
        }

        [TestMethod]
        public async Task GetShowImages_ReturnsHttpOk()
        {
            VerifyStatusCode(await _api.GetShowImagesAsync(ExistingShowId));
        }

        private void VerifyStatusCode<T>(Response<T> response)
        {
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}