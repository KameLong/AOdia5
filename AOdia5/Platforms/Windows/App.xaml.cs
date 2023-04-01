using KeyboardHookLite;
using Microsoft.UI.Xaml;
using System.Diagnostics;
using System.Windows.Input;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AOdia5.WinUI;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : MauiWinUIApplication
{
    KeyboardHook kbh=null;
    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        this.InitializeComponent();
    }
    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);
        //ここでキーボードのイベントを登録する
        Debug.WriteLine("OnLaunched");
       kbh = new KeyboardHook();
        kbh.KeyboardPressed += OnKeyPress;
    }
    private void OnKeyPress(object? sender, KeyboardHookEventArgs e)
    {
        /// <summary>
        /// The VirtualCode converted to typeof(Keys) for higher usability.
        /// </summary>
        /// 
        Key key= KeyInterop.KeyFromVirtualKey(e.InputEvent.VirtualCode);
        AOdiaKeyBoard.OnKeyPress(convertKey(key), convertKeyPressType(e.KeyPressType)); ;

    }
    private AOdiaKeyPressType convertKeyPressType(KeyboardHook.KeyPressType type)
    {
        switch (type)
        {
            case KeyboardHook.KeyPressType.KeyDown:
                return AOdiaKeyPressType.Down;
            case KeyboardHook.KeyPressType.KeyUp:
                return AOdiaKeyPressType.Up;
            case KeyboardHook.KeyPressType.SysKeyDown:
//                return AOdiaKeyPressType.Down;
            case KeyboardHook.KeyPressType.SysKeyUp:
            //                return AOdiaKeyPressType.Up;
            default:
                return (AOdiaKeyPressType)5;


        }
    }
    private AOdiaKey convertKey(Key type)
    {
        switch (type)
        {
            case Key.RightShift:
                return AOdiaKey.LeftShift;
            case Key.RightCtrl:
                return AOdiaKey.LeftCtrl;
            case Key.OemMinus:
                return AOdiaKey.Subtract;
        }

        return (AOdiaKey)type;
    }


    protected override MauiApp CreateMauiApp()
    {
        return MauiProgram.CreateMauiApp();
    }
}

