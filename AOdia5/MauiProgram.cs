using AOdiaData;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;
using Syncfusion.Maui.Core.Hosting;
using System.Runtime.CompilerServices;

namespace AOdia5;

public interface RecyclePage
{
    public Func<Page> Creater();
}
public static class MyExtensions
{
    public static void Goto(this Shell shell,string newURL,string? oldURL=null)
    {
        if(oldURL == null)
        {
            oldURL = UndoStack.Instance.RecentUrl();

        }


        UndoCommand undoCommand = new UndoCommand();
        undoCommand.comment = $"{oldURL}→{newURL}";
        undoCommand.Invoke = () =>
        {
            UndoStack.Instance.PushURL(newURL);
            shell.GoToAsync(newURL);
        };
        undoCommand.Redo = () =>
        {
            UndoStack.Instance.PushURL(newURL);
            shell.GoToAsync(newURL);
        };
        undoCommand.Undo = () =>
        {
            UndoStack.Instance.PushURL(oldURL);
            shell.GoToAsync(oldURL);
        };
        UndoStack.Instance.Push(undoCommand);

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
           .ConfigureSyncfusionCore()
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






