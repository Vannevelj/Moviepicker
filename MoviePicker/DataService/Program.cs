﻿using System;
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

            var tvGenres = await api.GetTvGenres();
            Console.WriteLine("TV Genres:\n" + string.Join("\n", tvGenres.Select(x => x.Name)));
            Console.WriteLine();

            var movieGenres = await api.GetMovieGenres();
            Console.WriteLine("Movie Genres:\n" + string.Join("\n", movieGenres.Select(x => x.Name)));
        }
    }
}