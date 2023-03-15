using AOdiaData;


namespace AOdia5;

public partial class App : Application
{
    //https://learn.microsoft.com/ja-jp/dotnet/maui/user-interface/pages/flyoutpage?view=net-maui-7.0

    public App()
	{
		InitializeComponent();
        MainPage = new HelloSkia();
		
	}
    protected override Window CreateWindow(IActivationState activationState)
    {
        Window window = base.CreateWindow(activationState);
        window.Stopped += (s, e) => 
        {
            DiaFile.staticDia.SaveChanges();
        };
        return window;
    }

}
