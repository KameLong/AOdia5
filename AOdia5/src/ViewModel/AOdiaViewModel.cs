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
    public class StationListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
          => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        //Modelのインスタンスを保持するプロパティ
        private readonly ObservableCollection<Station> _stations;
        public ObservableCollection<Station> Stations { get { return _stations; } }

        public StationListViewModel()
        {
            _stations = new ObservableCollection<Station>(DiaFile.staticDia.stations);
            _stations.CollectionChanged+=OnPropertyChanged;
        }
        internal Station AddNewStation()
        {
            Station station=new Station();
            station.Name.Value = "New Station";
            station.Lat.Value = 35;
            station.Lon.Value = 135;

            _stations.Add(station);
//            AOdiaData.AOdiaData.staticDia.stations.Add(station);
            return station;
        }
        internal void RemoveStation(Station station)
        {
//            AOdiaData.AOdiaData.staticDia.stations.Remove(station);
            _stations.Remove(station);
        }

        private void OnPropertyChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Notify("Stations");
        }
        protected void Notify(string propName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

    }
    public class EditStationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
          => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public Station editStation { get; set; }
        public StationListViewModel stationListViewModel { get; set; }

    }
}
