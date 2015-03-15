using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Movies;
using Newtonsoft.Json;

namespace Tests.TMDbWrapper
{
    [TestClass]
    public class JsonDeserializingTests
    {
        [TestMethod]
        [TestCategory("Unit_JSON")]
        public void DeserializingMovieObject_Works()
        {
            // Arrange
            const string json =
                "{\"adult\":false,\"backdrop_path\":\"/backdrop1.jpg\",\"belongs_to_collection\":{\"id\":8384,\"name\":\"Proletariat Collection\",\"poster_path\":null,\"backdrop_path\":\"/backdrop2.jpg\"},\"budget\":0,\"genres\":[{\"id\":18,\"name\":\"Drama\"},{\"id\":10769,\"name\":\"Foreign\"}],\"homepage\":\"\",\"id\":2,\"imdb_id\":\"tt0094675\",\"original_language\":\"en\",\"original_title\":\"Ariel\",\"overview\":\"Very long description...\",\"popularity\":0.0442581290706691,\"poster_path\":\"/poster.jpg\",\"production_companies\":[{\"name\":\"Villealfa Filmproduction Oy\",\"id\":2303},{\"name\":\"Finnish Film Foundation\",\"id\":2396}],\"production_countries\":[{\"iso_3166_1\":\"FI\",\"name\":\"Finland\"}],\"release_date\":\"1988-10-21\",\"revenue\":0,\"runtime\":69,\"spoken_languages\":[{\"iso_639_1\":\"de\",\"name\":\"Deutsch\"},{\"iso_639_1\":\"fi\",\"name\":\"suomi\"}],\"status\":\"Released\",\"tagline\":\"\",\"title\":\"Ariel\",\"video\":false,\"vote_average\":6.5,\"vote_count\":5}";

            // Act
            var movie = JsonConvert.DeserializeObject<Movie>(json);

            // Assert
            movie.IsAdult.Should().BeFalse();
            movie.BackdropPath.Should().Be("/backdrop1.jpg");
            movie.Budget.Should().Be(0);
            movie.Genres.Should().BeEquivalentTo(new Genre(18, "Drama"), new Genre(10769, "Foreign"));
            movie.Homepage.Should().Be(string.Empty);
            movie.TmdbId.Should().Be(2);
            movie.ImdbId.Should().Be("tt0094675");
            movie.OriginalTitle.Should().Be("Ariel");
            movie.OriginalLanguage.Should().Be("en");
            movie.Overview.Should().Be("Very long description...");
            movie.Popularity.Should().Be(0.0442581290706691);
            movie.PosterPath.Should().Be("/poster.jpg");
            movie.ReleaseDate.Should().Be(new DateTime(1988, 10, 21));
            movie.Revenue.Should().Be(0);
            movie.Runtime.Should().Be(69);
            movie.Languages.Should().BeEquivalentTo(new Language("de", "Deutsch"), new Language("fi", "suomi"));
            movie.Status.Should().Be("Released");
            movie.Tagline.Should().Be(string.Empty);
            movie.Title.Should().Be("Ariel");
            movie.HasVideo.Should().BeFalse();
            movie.VoteAverage.Should().Be(6.5);
            movie.VoteCount.Should().Be(5);
        }

        [TestMethod]
        [TestCategory("Unit_JSON")]
        public void DeserializingShowObject_Works()
        {
            // Arrange
            const string json =
                "{\"backdrop_path\":\"/backdrop1.jpg\",\"created_by\":[{\"id\":56157,\"name\":\"Terry Turner\",\"profile_path\":null},{\"id\":56156,\"name\":\"Bonnie Turner\",\"profile_path\":null}],\"episode_run_time\":[30,22],\"first_air_date\":\"1996-01-09\",\"genres\":[{\"id\":35,\"name\":\"Comedy\"}],\"homepage\":\"http://www.3rdrock.com/\",\"id\":155,\"in_production\":false,\"languages\":[\"en\"],\"last_air_date\":\"2001-05-22\",\"name\":\"3rd Rock from the Sun\",\"networks\":[{\"id\":6,\"name\":\"NBC\"}],\"number_of_episodes\":139,\"number_of_seasons\":6,\"origin_country\":[\"US\"],\"original_language\":\"en\",\"original_name\":\"3rd Rock from the Sun\",\"overview\":\"longass description\",\"popularity\":0.310595196481398,\"poster_path\":\"/qz4dIViqDakc9ovpQL3T1rNSMeH.jpg\",\"production_companies\":[],\"seasons\":[{\"air_date\":\"1996-01-09\",\"episode_count\":20,\"id\":59490,\"poster_path\":null,\"season_number\":1},{\"air_date\":\"1996-09-22\",\"episode_count\":26,\"id\":59493,\"poster_path\":null,\"season_number\":2},{\"air_date\":\"1997-09-24\",\"episode_count\":27,\"id\":59494,\"poster_path\":null,\"season_number\":3},{\"air_date\":\"1998-09-23\",\"episode_count\":24,\"id\":59514,\"poster_path\":null,\"season_number\":4},{\"air_date\":\"1999-09-22\",\"episode_count\":22,\"id\":59515,\"poster_path\":null,\"season_number\":5},{\"air_date\":\"2000-10-24\",\"episode_count\":20,\"id\":59516,\"poster_path\":null,\"season_number\":6}],\"status\":\"Ended\",\"type\":\"Scripted\",\"vote_average\":5.8,\"vote_count\":4}";

            // Act
            var show = JsonConvert.DeserializeObject<Show>(json);

            // Assert
            show.BackdropPath.Should().Be("/backdrop1.jpg");
            show.FirstAiring.Should().Be(new DateTime(1996, 01, 09));
            show.Genres.Should().BeEquivalentTo(new Genre(35, "Comedy"));
            show.Homepage.Should().Be("http://www.3rdrock.com/");
            show.TmdbId.Should().Be(155);
            show.InProduction.Should().BeFalse();
            show.Languages.Should().BeEquivalentTo(new Language("en", string.Empty));
            show.LastAiring.Should().Be(new DateTime(2001, 05, 22));
            show.Name.Should().Be("3rd Rock from the Sun");
            show.AmountOfEpisodes.Should().Be(139);
            show.AmountOfSeasons.Should().Be(6);
            show.OriginalLanguage.Should().Be("en");
            show.OriginalName.Should().Be("3rd Rock from the Sun");
            show.Overview.Should().Be("longass description");
            show.Popularity.Should().Be(0.310595196481398);
            show.PosterPath.Should().Be("/qz4dIViqDakc9ovpQL3T1rNSMeH.jpg");
            show.Status.Should().Be("Ended");
            show.Type.Should().Be("Scripted");
            show.AverageVote.Should().Be(5.8);
            show.AmountOfVotes.Should().Be(4);
        }
    }
}