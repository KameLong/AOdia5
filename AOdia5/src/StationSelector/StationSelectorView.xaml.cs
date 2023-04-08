using AOdiaData;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AOdia5;


public class OnStationSelectedEventArgs:EventArgs
{
    public Station station;

}
public partial class StationSelectorView : ContentView
{
    public delegate void OnSelected(OnStationSelectedEventArgs args);
    public OnSelected onSelected;

    private StationSelectorViewModel VM { get { return (StationSelectorViewModel)BindingContext; } }

	public StationSelectorView()
	{
		this.BindingContext = new StationSelectorViewModel();
		InitializeComponent();
	}
    private void SelectedStation(object sender, SelectedItemChangedEventArgs e)
    {
        ListView listView = (ListView)sender;
        if (onSelected != null)
        {
            var args = new OnStationSelectedEventArgs();
            args.station = (Station)listView.SelectedItem;
            onSelected(args);
        }
    }

    private void OnStationClicked(object sender, TappedEventArgs e)
    {
        if (sender is HorizontalStackLayout layout && layout.BindingContext is VMStation station)
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

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        VM.searchText = e.NewTextValue;
        VM.OnPropertyChanged(nameof(VM.Stations));
    }
}
public class StationSelectorViewModel : INotifyPropertyChanged
{
    public string searchText = "";
    public event PropertyChangedEventHandler? PropertyChanged;
    public virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
      => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    //Modelのインスタンスを保持するプロパティ
    private readonly ObservableCollection<Station> _stations;
    public List<VMStation> Stations
    {
        get
        {
            var stationNameContained = DiaFile.staticDia.stations.Where(s => s.DbName.Contains(searchText)).OrderBy(s => s.DbName).Select(s => new VMStation(s));
            return stationNameContained.ToList();
        }
    }

    public StationSelectorViewModel()
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

public class VMStation
{
    private Station station;
    public VMStation(Station station)
    {
        this.station = station;
    }
    public String name { get { return station.DbName; } }
    public String lat { get { return station.DbLat.ToString("F4"); } }
    public String lon { get { return station.DbLon.ToString("F4"); } }
    public String routes
    {
        get
        {
            var path1 = DiaFile.staticDia.paths.Where(p => p.endStationID == station.StationId).ToList();
            var path2 = DiaFile.staticDia.paths.Where(p =>
                p.startStationID == station.StationId && p.seq == 0
                ).ToList();
            path1.AddRange(path2);
            var str = "";
            foreach (var p in path1)
            {
                str += " " + p.route.Name;
            }

            return str;
        }
    }
}
