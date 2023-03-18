using CommunityToolkit.Maui.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOdia5
{
    class LabelButton:Button
    {
        public LabelButton()
        {

            var g = new PointerGestureRecognizer();
            g.PointerEntered += onPointerEnter;
            g.PointerExited += onPointerExit;
            this.GestureRecognizers.Add(g);
        }
        private async void onPointerEnter(object? obj,PointerEventArgs args)
        {
            Debug.WriteLine("onPointerEnter")
                ;
//            this.BackgroundColorTo( (Color)Application.Current.Resources.MergedDictionaries.ElementAt(0)["Primary50"]);
//            await this.BackgroundColorTo(Colors.LightBlue,16,2000);
//            await this.BackgroundColorTo(Colors.White, 16, 2000);

        }
        private void onPointerExit(object? obj, PointerEventArgs args)
        {
            this.BackgroundColor = Colors.Transparent;

        }
    }
}
