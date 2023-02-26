using System.Diagnostics;

namespace AOdia5;

public class TestPage : ContentPage
{
	public TestPage()
	{
		Content = new VerticalStackLayout
		{
			Children = {
				new Label { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Text = "Welcome to .NET MAUI!"
				}
			}
		};
		var now=DateTime.Now;
		Debug.WriteLine("");

	}
}