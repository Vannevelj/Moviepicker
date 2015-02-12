using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using TMDbWrapper;

namespace DataService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new Program().StartProgram();
            Console.Read();
        }

        public async Task StartProgram()
        {
            var api = new TMDbApi(ConfigurationManager.AppSettings["apikey"]);

            var tvGenres = await api.GetShowGenresAsync();
            Console.WriteLine("TV Genres:\n" + string.Join("\n", tvGenres.Data.Select(x => x.Name)));
            Console.WriteLine();

            var movieGenres = await api.GetMovieGenresAsync();
            Console.WriteLine("Movie Genres:\n" + string.Join("\n", movieGenres.Data.Select(x => x.Name)));
            Console.WriteLine();

            var movieDetails = await api.GetMovieAsync(458);
            Console.WriteLine(movieDetails.Data.Title);
            Console.WriteLine();

            var keywords = await api.GetMovieKeywordsAsync(movieDetails.Data.TmdbId);
            Console.WriteLine("Keywords:\n" + string.Join("\n", keywords.Data.Select(x => x.Name)));
        }
    }
}