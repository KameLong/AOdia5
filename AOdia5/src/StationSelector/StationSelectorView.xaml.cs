using AOdiaData;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AOdia5;


public class OnStationSelectedEventArgs:EventArgs
{
    public Station station;
    public OnStationSelectedEventArgs(Station station)
    {
        this.station = station;
    }

}
public partial class StationSelectorView : ContentView, INotifyPropertyChanged
{
    public event EventHandler OnStationSelected;

    public event PropertyChangedEventHandler? PropertyChanged;
    public virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
      => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


    public string searchText = "";
    public List<VMStation> _stations = new List<VMStation>();
    public List<VMStation> Stations
    {
        get
        {
                _stations=DiaFile.staticDia.stations.Where(s => s.DbName.Contains(searchText)).OrderBy(s => s.DbName).Select(s => new VMStation(s)).ToList();
            return _stations;
            var stationNameContained = DiaFile.staticDia.stations.Where(s => s.DbName.Contains(searchText)).OrderBy(s => s.DbName).Select(s => new VMStation(s));
            return stationNameContained.ToList();
        }
    }
    private bool LoadMoreStation()
    {
        return true;
        _stations.AddRange(DiaFile.staticDia.stations.Where(s => s.DbName.Contains(searchText)).OrderBy(s => s.DbName).Skip(_stations.Count).Take(30).Select(s => new VMStation(s)));
        OnPropertyChanged(nameof(this.Stations));
    }




    public StationSelectorView()
	{
		InitializeComponent();
        this.BindingContext = this;
    }


    private void OnStationClicked(object sender, TappedEventArgs e)
    {
        if (sender is HorizontalStackLayout layout && layout.BindingContext is VMStation station)
        {
                var args = new OnStationSelectedEventArgs(station.station);
                OnStationSelected?.Invoke(this, args);
        }
    }


    private void SearchBarTextChanged(object sender, TextChangedEventArgs e)
    {
        searchText = e.NewTextValue;
        OnPropertyChanged(nameof(Stations));
    }

    private void OnRemainingItemsThresholdReached(object sender, EventArgs e)
    {
        bool hasMoreItems = LoadMoreStation();
    }
}


public class VMStation
{
    public Station station;
    public VMStation(Station station)
    {
        this.station = station;
    }

    public String name { get { return station.DbName; } }
    public String lat { get { return station.DbLat.ToString("F4"); } }
    public String lon { get { return station.DbLon.ToString("F4"); } }
    public List<string> routes2
    {
        get
        {
            var path1 = DiaFile.staticDia.paths.Where(p => p.endStationID == station.StationId).ToList();
            var path2 = DiaFile.staticDia.paths.Where(p =>
                p.startStationID == station.StationId && p.seq == 0
                ).ToList();
            path1.AddRange(path2);

            return path1.Select(p=>p.route.Name).ToList();
        }
    }
}
