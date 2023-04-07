using AOdiaData;
using CommunityToolkit.Maui.Views;
using Reactive.Bindings;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;

namespace AOdia5;




public partial class StationListPage : ContentPage
{
    private StationListViewModel VM { get { return (StationListViewModel)BindingContext; } }
    public StationListPage()
	{
        
        InitializeComponent();
        BindingContext = new StationListViewModel();
    }

    private void SelectedStation(object sender, SelectedItemChangedEventArgs e)
    {
        // リストビューで選択されたアイテムを取得する。
        ListView listView = (ListView)sender;
        Station station = (Station)listView.SelectedItem;
        EditStationViewModel vm = new EditStationViewModel { editStation = station, stationListViewModel = VM };
        this.ShowPopup(new EditStationModal(vm,Navigation));
    }
    private void OnClickAddStation(object sender, TappedEventArgs e)
    {
        Station station=VM.AddNewStation();
        EditStationViewModel vm= new EditStationViewModel { editStation = station, stationListViewModel = VM };
        this.ShowPopup(new EditStationModal(vm,Navigation));
    }

    private void OnStationClicked(object sender, TappedEventArgs e)
    {
        if(sender is HorizontalStackLayout layout&&layout.BindingContext is VMStation station)
        {
            Debug.WriteLine(station.name);
        }
    }

    private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is HorizontalStackLayout layout && layout.BindingContext is VMStation station)
        {
            Debug.WriteLine(station.name);
        }
    }
}

public class VMStation
{
    private Station station;
    public VMStation(Station station)
    {
        this.station = station;
    }
    public String name { get{ return station.DbName; } }
    public String lat { get { return station.DbLat.ToString("F4"); } }
    public String lon { get { return station.DbLon.ToString("F4"); } }
    public String routes {get
        {
            var path1=DiaFile.staticDia.paths.Where(p => p.endStationID == station.StationId).ToList();
            var path2 = DiaFile.staticDia.paths.Where(p =>
                p.startStationID == station.StationId && p.seq == 0
                ).ToList();
            path1.AddRange(path2);
            var str = "";
            foreach(var p in path1)
            {
                str += " " + p.route.Name;
            }

            return str;
        }
    }
}
public class StationListViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
      => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    //Modelのインスタンスを保持するプロパティ
    private readonly ObservableCollection<Station> _stations;
    public List<VMStation> Stations { get {
            var a = DiaFile.staticDia.stations.OrderBy(s => s.DbName);
           return  a.Select(s => new VMStation(s)).ToList();
        } }

    public StationListViewModel()
    {
        _stations = new ObservableCollection<Station>(DiaFile.staticDia.stations);
        _stations.CollectionChanged += OnPropertyChanged;
    }
    internal Station AddNewStation()
    {
        Station station = new Station();
        station.Name.Value = "New Station";
        station.Lat.Value = 35;
        station.Lon.Value = 135;

        _stations.Add(station);
        return station;
    }
    internal void RemoveStation(Station station)
    {
        _stations.Remove(station);
    }

    private void OnPropertyChanged(object? sender, NotifyCollectionChangedEventArgs e)
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