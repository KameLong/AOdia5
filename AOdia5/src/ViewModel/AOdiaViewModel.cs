using AOdiaData;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

        //Modelのインスタンスを保持するプロパティ
        public List<Station> Stations { get; set; } = new List<Station>();

        public StationListViewModel()
        {
            if (StaticData.staticDia == null)
            {
                StaticData.staticDia = new DiaFile();
            }
            Stations = StaticData.staticDia.Stations.ToList();
        }

    }
    internal class EditStationViewModel
    {

        public Station editStation { get; set; } = new Station();
        public StationListViewModel stationListViewModel { get; set; }

    }
}
