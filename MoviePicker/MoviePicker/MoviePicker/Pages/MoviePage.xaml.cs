using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Shared.Models;

namespace MoviePicker
{	
	public partial class MoviePage : ContentPage
	{	
		private readonly Movie _movie;

		public MoviePage (Movie movie)
		{
			_movie = movie;
			InitializeComponent();

			_title.Text = string.Format ("{0}({1})", _movie.Title, _movie.ReleaseDate.HasValue ? _movie.ReleaseDate.Value.Year.ToString () : "Unknown");
			_image.Source = new UriImageSource {
				CachingEnabled = true,
				CacheValidity = new TimeSpan(5, 0, 0, 0),
				Uri = new Uri(_movie.PosterPath)
			};
			_description.Text = _movie.Overview;

			// Main layout: Padding = new Thickness(0, 0, 0, 0)
		}

		private void OnMoreInformationClicked(object sender, EventArgs e) { }
		private void OnDeclineClicked(object sender, EventArgs e) { }
		private void OnMaybeClicked(object sender, EventArgs e) { }
		private void OnWatchedClicked(object sender, EventArgs e) { }
	}
}

