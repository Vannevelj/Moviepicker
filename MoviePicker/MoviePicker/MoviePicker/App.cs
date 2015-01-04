using MoviePicker.Models;
using Xamarin.Forms;

namespace MoviePicker
{
	public class App
	{
		public static Page GetMainPage()
		{
		    var session = new Session();
			return new NavigationPage(new WelcomePage(session));
		}
	}
}
