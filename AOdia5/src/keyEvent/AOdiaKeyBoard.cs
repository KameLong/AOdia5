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
    public class AOdiaKeyBoard
    {
        public KeyboardHook kbh;
        public AOdiaKeyBoard()
        {
//            kbh = new KeyboardHook();
//            kbh.KeyboardPressed += OnKeyPress;
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


    public interface AOdiaKeyEvent
    {

    }
}
