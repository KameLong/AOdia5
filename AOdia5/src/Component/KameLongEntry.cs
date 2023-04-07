using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOdia5
{
    public class KameLongEntry:Entry
    {
        private string? temp = null;
        public KameLongEntry() {
            this.Focused += KameLongEntry_Focused;
            this.Unfocused += KameLongEntry_Unfocused;
        }
        public Func<string, bool> ValueCheck { get; set; } = null;
        public Action<string?> OnUnFocused;

        private void KameLongEntry_Unfocused(object? sender, FocusEventArgs e)
        {
            if (OnUnFocused == null)
            {
                return;
            }
            if(ValueCheck == null)
            {
                OnUnFocused(this.Text);
                return;
            }
            if (ValueCheck(this.Text))
            {
                OnUnFocused(this.Text);
                return;
            }
            OnUnFocused(null);
            this.Text = this.temp;
            return;

        }

        private void KameLongEntry_Focused(object? sender, FocusEventArgs e)
        {
            temp = this.Text;
        }
    }
}
