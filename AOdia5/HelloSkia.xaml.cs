namespace AOdia5;

public partial class HelloSkia : ContentPage
{
	public HelloSkia()
	{
		InitializeComponent();
        if (Device.Idiom == TargetIdiom.Phone)
        {
            Shell.Current.FlyoutIsPresented = false;
        }

    }
}