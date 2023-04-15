using AOdiaData;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace AOdia5;

public interface RecyclePage
{
    public Func<Page> Creater();
}
public static class MyExtensions
{
    public static void Goto(this Shell shell,string newURL,string? oldURL=null)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            // Code to run on the main thread

        if (oldURL == null)
        {
            oldURL = UndoStack.Instance.RecentUrl();

        }


        UndoCommand undoCommand = new UndoCommand();
        undoCommand.comment = $"{oldURL}→{newURL}";
        undoCommand.Invoke = async() =>
        {
            UndoStack.Instance.PushURL(newURL);
            Page oldPage=shell.CurrentPage;
            await shell.GoToAsync(newURL, false);
        };
        undoCommand.Redo = async() =>
        {
            UndoStack.Instance.PushURL(newURL);
            await shell.Navigation.PopAsync(false);
            await shell.GoToAsync(newURL, false);
        };
        undoCommand.Undo = async() =>
        {
            UndoStack.Instance.PushURL(oldURL);
            await shell.Navigation.PopAsync(false);
            await shell.GoToAsync(oldURL, false);
        };
        UndoStack.Instance.Push(undoCommand);
        });

    }
}


public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
        builder
           .UseSkiaSharp(true)
           .UseMauiApp<App>()
           .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont(filename: "materialdesignicons-webfont.ttf", alias: "MaterialDesignIcons");
            });



#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}






