using AOdiaData;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AOdia5
{
    internal class AOdiaViewModel
    {
        public DiaFile diaFile { get; set; }
    }

    public class MapViewModel
    {
        public ICollection<Station> stations { get; set; }
        public EditStationViewModel? editStation { get; set; }
    }
 
    public class EditStationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
          => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public Station? editStation { get; set; }
        public StationSelectorViewModel? stationListViewModel { get; set; }

    }
}
