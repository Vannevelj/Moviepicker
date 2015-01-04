using System;
using System.Collections.Generic;
using Xamarin.Forms;
using MoviePicker.Models;
using Acr.XamForms.UserDialogs;
using Acr.XamForms.UserDialogs.Droid;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json;
using Shared.Models;
using System.Threading.Tasks;

namespace MoviePicker
{	
	public partial class LoginPage : ContentPage
	{	
		private readonly Session _session;

		public LoginPage(Session session)
		{
			_session = session;
			InitializeComponent();
		}

		private async void OnLoginButtonClicked(object sender, EventArgs args)
		{
			_activityIndicator.IsRunning = true;
			var isValidTask = IsValidLogin();
			var isValid = await isValidTask;

			if (isValid)
			{
				var genres = await _session.DataSource.GetGenresAsync();
				await Navigation.PushAsync(new GenrePage(_session, genres));
			}
			else
			{
				var alert = new AlertConfig
				{
					Message = "The entered credentials are invalid",
					Title = "Invalid login",
					OkText = "Okay then.."
				};
				var dialogService = new UserDialogService();
				await dialogService.AlertAsync(alert);
			}

			if (isValidTask.IsCompleted)
			{
				_activityIndicator.IsRunning = false;
			}
		}

		private async Task<bool> IsValidLogin()
		{
			var email = _emailEntry.Text;
			var password = _passwordEntry.Text;

			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
					{
						new KeyValuePair<string, string>("email", email),
						new KeyValuePair<string, string>("password", password)
					});

				HttpResponseMessage response;
				try
				{
					response = await client.PostAsync("http://mpdev.azurewebsites.net/api/users/login", content);

					var json = await response.Content.ReadAsStringAsync();
					var obj = JsonConvert.DeserializeObject<User>(json);
					_session.User = obj;
				}
				catch (Exception e)
				{
					//TODO: log
					var alert = new AlertConfig
					{
						Message = "Something went wrong with your request, try again later.",
						Title = "Uh oh",
						OkText = "Fk dis app"
					};
					var dialogService = new UserDialogService();
					dialogService.AlertAsync(alert);

					return false;
				}

				return response.IsSuccessStatusCode;
			}
		}
	}
}

