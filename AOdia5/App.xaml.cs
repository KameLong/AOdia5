using AOdia5.src.Test;
using AOdiaData;
using KeyboardHookLite;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AOdia5;
public partial class App : Microsoft.Maui.Controls.Application
{
    //https://learn.microsoft.com/ja-jp/dotnet/maui/user-interface/pages/flyoutpage?view=net-maui-7.0

    public App()
	{
        InitializeComponent();
        MainPage = new StationSelectorPage();
	}
    protected override Microsoft.Maui.Controls.Window CreateWindow(IActivationState? activationState)
    {
        Debug.WriteLine("CreateWindow");
        Window window = base.CreateWindow(activationState);

        AOdiaKeyBoard.Init(window,MainPage);

        window.Stopped += (s, e) => 
        {
            Debug.WriteLine("Stopeed");
            DiaFile.staticDia.SaveChanges();
        };

        
        return window;
    }

}




