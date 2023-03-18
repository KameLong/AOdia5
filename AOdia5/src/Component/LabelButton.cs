using CommunityToolkit.Maui.Extensions;
using Microsoft.Maui.Controls.Platform.Compatibility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOdia5
{
    class KLButton:Button
    {
        public event EventHandler Tapped;
        public KLButton()
        {

            var g = new PointerGestureRecognizer();
            g.PointerEntered += onPointerEnter;
            g.PointerExited += onPointerExit;
            var t=new TapGestureRecognizer();
            t.Tapped += onClick;
#if WINDOWS
            this.GestureRecognizers.Add(g);
#endif
            this.GestureRecognizers.Add(t);
        }
        private void onClick(object? obj,TappedEventArgs args)
        {
            Task.Run(async () =>
            {
                await this.BackgroundColorTo(Colors.LightBlue, 16, 300);
                await this.BackgroundColorTo(Colors.Transparent, 16, 300);

            });
            Tapped?.Invoke(this, args);

        }
        private void onPointerEnter(object? obj,PointerEventArgs args)
        {
            this.BackgroundColor= (Color)Application.Current.Resources.MergedDictionaries.ElementAt(0)["Primary50"];
//            this.BackgroundColorTo( (Color)Application.Current.Resources.MergedDictionaries.ElementAt(0)["Primary50"],16,16);

        }
        private void onPointerExit(object? obj, PointerEventArgs args)
        {
            this.BackgroundColor=Colors.Transparent;

        }
    }
}
