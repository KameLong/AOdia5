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
        MainPage = new MainPage();
	}
    protected override Microsoft.Maui.Controls.Window CreateWindow(IActivationState? activationState)
    {


//        AOdiaKeyBoard keyEvent = new AOdiaKeyBoard();

        Window window = base.CreateWindow(activationState);
        KeyboardHook kbh=null;
        window.Created += (s, e) =>
        {
            if (kbh == null)
            {
                kbh = new KeyboardHook();
                kbh.KeyboardPressed += OnKeyPress;

            }

        };
        window.Stopped += (s, e) => 
        {
            Debug.WriteLine("Stopeed");
            DiaFile.staticDia.SaveChanges();
        };
        window.Resumed += (s, e) =>
        {
            if (kbh == null)
            {
                kbh = new KeyboardHook();
                kbh.KeyboardPressed += OnKeyPress;

            }

            Debug.WriteLine("Resumed");
        };

        
        return window;
    }
    private void OnKeyPress(object? sender, KeyboardHookEventArgs e)
    {

        if (e.KeyPressType == KeyboardHook.KeyPressType.KeyDown)
        {
            Debug.WriteLine(e.InputEvent.VirtualCode);
            Debug.WriteLine(e.InputEvent.HardwareScanCode);
            Debug.WriteLine(e.InputEvent.Flags);
            Debug.WriteLine(e.InputEvent.AdditionalInformation);
#if WINDOWS
            //            Debug.WriteLine(e.InputEvent.Key            );

#endif
            Debug.WriteLine("");

        }
    }

}




