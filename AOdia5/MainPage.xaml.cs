namespace AOdia5;
using AOdiaData;
using System.Diagnostics;

public partial class MainPage : FlyoutPage
{

	public MainPage()
	{
		InitializeComponent();
    }

    private void NavigationPage_Popped(object sender, NavigationEventArgs e)
    {
        Debug.WriteLine(e);
    }

    private void NavigationPage_Pushed(object sender, NavigationEventArgs e)
    {
        Debug.WriteLine(e);

    }
}

