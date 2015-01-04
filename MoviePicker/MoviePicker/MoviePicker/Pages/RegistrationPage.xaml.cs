using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using Shared.Models;
using System.Net;
using Acr.XamForms.UserDialogs;
using Acr.XamForms.UserDialogs.Droid;
using MoviePicker.Models;

namespace MoviePicker
{	
	public partial class RegistrationPage : ContentPage
	{	
		private readonly Session _session;

		public RegistrationPage (Session session)
		{
			_session = session;
			InitializeComponent();
		}

		private async void OnRegistrationButtonClicked(object sender, EventArgs e)
		{
		    _activityIndicator.IsRunning = true;

		    var response = await _session.DataSource.RegisterAsync(_email.Text, _password.Text, _firstname.Text, _lastname.Text);
		    if (response.IsSuccessStatusCode)
		    {
		        var json = await response.Content.ReadAsStringAsync();
		        var obj = JsonConvert.DeserializeObject<User>(json);
		        _session.User = obj;

		        var genres = await _session.DataSource.GetGenresAsync();
		        await Navigation.PushAsync(new GenrePage(_session, genres));
		    }
		    else
		    {
		        if (response.StatusCode == HttpStatusCode.Conflict)
		        {
		            var alert = await DisplayAlert("User exists",
		                "A user with this email address already exists",
		                "Go to login",
		                "Cancel");

		            // User clicked on "Go to login"
		            if (alert)
		            {
		                await Navigation.PushAsync(new LoginPage(_session));
		            }
		        }
		        else
		        {
                    var alert = new AlertConfig
                    {
                        Message = "Something went wrong with your request, try again later.",
                        Title = "Uh oh",
                        OkText = "Fk dis app"
                    };
                    new UserDialogService().AlertAsync(alert);
		        }
		    }
 
		    _activityIndicator.IsRunning = false;  
		}
	}
}

