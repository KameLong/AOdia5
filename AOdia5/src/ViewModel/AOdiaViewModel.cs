using AOdiaData;
using Microsoft.EntityFrameworkCore.Infrastructure;
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

    internal class MapViewModel
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
            if (StaticData.staticDia == null)
            {
                StaticData.staticDia = new DiaFile();
            }
            _stations = new ObservableCollection<Station>(StaticData.staticDia.Stations);
            _stations.CollectionChanged+=OnPropertyChanged;
        }
        internal Station AddNewStation()
        {
            Station station=new Station();
            _stations.Add(station);
            StaticData.staticDia.Stations.Add(station);
            StaticData.staticDia.SaveChanges();
            return station;
        }
        internal void RemoveStation(Station station)
        {
            StaticData.staticDia.Stations.Remove(station);
            _stations.Remove(station);
            StaticData.staticDia.SaveChanges();
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
    internal class EditStationViewModel
    {

        public Station editStation { get; set; } = new Station();
        public StationListViewModel stationListViewModel { get; set; }

    }
}
