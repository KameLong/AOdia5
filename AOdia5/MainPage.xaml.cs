namespace AOdia5;
using AOdiaData;
using System.Diagnostics;

public partial class MainPage : FlyoutPage
{
    public static INavigation navigation;

	public MainPage()
	{
		InitializeComponent();
        navigation = nav.Navigation;
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

