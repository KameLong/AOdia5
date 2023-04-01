using System.Reflection.Metadata;

namespace AOdia5;

public class KeyEventTest : ContentPage, AOdiaKeyEvent
{
    private Label label;
	public KeyEventTest()
	{
        label=new Label
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Text = "Welcome to\n .NET MAUI!"
        };


        Content = new VerticalStackLayout
		{
			Children = {
                label,
			}
		};
	}

    public void OnKeyPress(AOdiaKey keyCode)
    {
        label.Text += "\n" + keyCode.ToString();
    }
}