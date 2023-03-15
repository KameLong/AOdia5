

using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace AOdia5.src.skTest;

public class SKtest : SKCanvasView
{
    protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
    {
        base.OnPaintSurface(e);

        var _canvas = e.Surface.Canvas;
        _canvas.Clear(); // clears the canvas for every frame
        var _info = e.Info;
        var _drawRect = new SKRect(0, 0, _info.Width, _info.Height);

        using var basePath = new SKPath();

        basePath.AddRect(_drawRect);

        _canvas.DrawPath(basePath, new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color =Colors.Red.ToSKColor(),
            IsAntialias = true
        });
        //...
    }
}