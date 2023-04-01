using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using KeyboardHookLite;

namespace AOdia5;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{

public override bool DispatchKeyEvent(KeyEvent? e)
    {
        System.Diagnostics.Debug.WriteLine(e.KeyCode);
        Keycode keycode = e.KeyCode;
        AOdiaKeyBoard.OnKeyPress(keyConverter(keycode),(AOdiaKeyPressType)e.Action);
        return base.DispatchKeyEvent(e);
    }
    private AOdiaKey keyConverter(Keycode code)
    {
        if (code >= Keycode.A && code <= Keycode.Z)
        {
            return code - Keycode.A + AOdiaKey.A;
        }
        if (code == Keycode.Enter)
        {
            return AOdiaKey.Enter;
        }
        if (code == Keycode.Del)
        {
            return AOdiaKey.Delete;
        }
        if (code == Keycode.Insert)
        {
            return AOdiaKey.Insert;
        }
        if (code == Keycode.Minus)
        {
            return AOdiaKey.Subtract;
        }

        if (code == Keycode.CtrlLeft|| code == Keycode.CtrlRight)
        {
            return AOdiaKey.LeftCtrl;
        }
        if (code == Keycode.CtrlLeft || code == Keycode.CtrlRight)
        {
            return AOdiaKey.LeftCtrl;
        }
        if (code == Keycode.ShiftLeft || code == Keycode.ShiftRight)
        {
            return AOdiaKey.LeftShift;
        }

        return AOdiaKey.None;

    }
}
