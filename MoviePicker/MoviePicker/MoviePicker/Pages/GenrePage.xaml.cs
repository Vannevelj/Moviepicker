using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Shared.Models;
using MoviePicker.Models;
using System.Linq;

namespace MoviePicker
{	
	public partial class GenrePage : ContentPage
	{	
		private readonly IEnumerable<Genre> _genres; 
		private readonly Session _session;

		public GenrePage(Session session, IEnumerable<Genre> genres)
		{
			_session = session;
			_genres = genres;
			InitializeComponent ();

			foreach (var genre in _genres) 
			{
				_genreCells.Add(new GenreCell(genre, _session, Navigation));
			}
		}
	}

	public class GenreCell : TextCell
	{
		private readonly Session _session;
		private readonly INavigation _navigation;
		public Genre Genre { get; set; }

		public GenreCell(Genre genre, Session session, INavigation navigation)
		{
			Genre = genre;
			_session = session;
			_navigation = navigation;
			Text = genre.Name;
			Tapped += TappedInternal;
		}

		private async void TappedInternal(object sender, EventArgs e)
		{
			var movie = (await _session.DataSource.GetMoviesForGenreAsync(Genre.Id)).Take(1).FirstOrDefault();
			var movieDetails = await _session.DataSource.GetMovieAsync(movie.Id);
			await _navigation.PushAsync(new MoviePage(movieDetails));
		}
	}
}

