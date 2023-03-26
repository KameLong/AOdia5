using Microsoft.Win32;
using SkiaSharp.Views.Maui.Controls.Compatibility;
using SkiaSharp.Views.Maui.Controls.Hosting;
using SkiaSharp.Views.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using KeyboardHookLite;

namespace AOdia5
{
    /*
     * AOdiaで使用するキーイベントを管理します
     */
    public static class AOdiaKeyBoard
    {
        private static KeyboardHook? kbh=null;

        private static bool windowIsActive = false;
        private static bool isCtrlPressed = false;

        public static Page mainPage;

        public static void Init(Window window, Page page)
        {
            window.Created += (s, e) =>
            {
                windowIsActive = true;
#if WINDOWS
                if (kbh == null)
                {
                    kbh = new KeyboardHook();
                    kbh.KeyboardPressed += OnKeyPress;
                }
#endif
            };
            window.Resumed += (s, e) =>
            {
                windowIsActive = true;
#if WINDOWS

                if (kbh == null)
                {
                    kbh = new KeyboardHook();
                    kbh.KeyboardPressed += OnKeyPress;
                }
#endif
            };
            window.Deactivated += (s, e) =>
            {
                windowIsActive = false;
            };

            window.Stopped += (s, e) =>
            {
#if WINDOWS
                windowIsActive = false;
                kbh.Dispose();
                kbh = null;
#endif
            };
            mainPage = page;
        }
        public static void OnKeyPress(int keyCode)
        {

            if (mainPage is MainPage page && page.CurrentPage is AOdiaKeyEvent keyEvent)
            {
                keyEvent.OnKeyPress(keyCode);
            }

        }
        private static void OnKeyPress(object? sender, KeyboardHookEventArgs e)
        {
            if (windowIsActive) { 


            if (e.KeyPressType == KeyboardHook.KeyPressType.KeyDown)
            {
                    OnKeyPress(e.InputEvent.VirtualCode);

            }
            }
        }


    }


    public interface AOdiaKeyEvent
    {
        public void OnKeyPress(int keyCode);
    }
}
