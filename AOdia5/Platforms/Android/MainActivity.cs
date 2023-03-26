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
        AOdiaKeyBoard.OnKeyPress(((int)e.KeyCode));


        return base.DispatchKeyEvent(e);
    }
}
