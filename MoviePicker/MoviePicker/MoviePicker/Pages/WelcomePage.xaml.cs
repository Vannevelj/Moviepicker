using System;
using System.Collections.Generic;
using Xamarin.Forms;
using MoviePicker.Models;

namespace MoviePicker
{	
	public partial class WelcomePage : ContentPage
	{	
		private readonly Session _session;

		public WelcomePage(Session session)
		{
			_session = session;
			InitializeComponent ();
		}

		private async void OnLoginButtonClicked(object sender, EventArgs e)
		{
			_activityIndicator.IsRunning = true;
			await Navigation.PushAsync(new LoginPage(_session));
			_activityIndicator.IsRunning = false;
		}

		private async void OnRegisterButtonClicked(object sender, EventArgs e)
		{
			_activityIndicator.IsRunning = true;
			await Navigation.PushAsync(new RegistrationPage(_session));
			_activityIndicator.IsRunning = false;
		}
	}
}

