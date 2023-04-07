using System.Reflection.Metadata;

namespace AOdia5;

public class KeyEventTest : ContentPage, KeyEventListener
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

    public void OnKeyPress(AOdiaKey keyCode,ModifierKey modifier)
    {
        label.Text += "\n" + keyCode.ToString();
    }
}