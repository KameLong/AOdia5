namespace AOdia5;


public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        if (Device.Idiom == TargetIdiom.Phone)
        {
            this.FlyoutBehavior = FlyoutBehavior.Flyout;
        }

    }
}
