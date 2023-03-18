using AOdiaData;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;
using Syncfusion.Maui.Core.Hosting;
using System.Runtime.CompilerServices;

namespace AOdia5;
public static class MyExtensions
{
    public static void PushPage(this INavigation nav,Func<Page> func)
    {
        UndoCommand undoCommand = new UndoCommand();
        undoCommand.Invoke = () =>
        {
            nav.PushAsync(func());
        };
        undoCommand.Redo = () =>
        {
            nav.PushAsync(func());
        };
        undoCommand.Undo = () =>
        {
            nav.PopAsync();
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
