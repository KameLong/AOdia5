using AOdiaData;
using CommunityToolkit.Maui.Core.Extensions;
using Mapsui.UI.Maui;
using Microsoft.EntityFrameworkCore;
using Reactive.Bindings;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AOdia5;

public partial class RouteEditFromMapPage : ContentPage
{
    public RouteEditFromMapPageModel VM { get { return BindingContext as RouteEditFromMapPageModel; } set { BindingContext = value; } }
	public RouteEditFromMapPage()
	{
        DateTime now=DateTime.Now;
		InitializeComponent();
        mapControl.Map?.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer());
        Debug.WriteLine($"{(DateTime.Now-now).TotalMilliseconds}  読み込み開始");
        VM = new RouteEditFromMapPageModel(AOdiaData.AOdiaData.routes.Include(r=>r.Paths).ToList(), AOdiaData.AOdiaData.staticDia.stations.Take(0).ToList());
        Debug.WriteLine($"{(DateTime.Now - now).TotalMilliseconds}  読み込み終了");


        drawStation();
        drawRoute();
        Debug.WriteLine($"{(DateTime.Now - now).TotalMilliseconds}  描画終了");
    }
    private void drawStation()
    {
        mapControl.Pins.Clear();
        int count = 0;
        foreach (var station in VM.stations)
        {
            count++;
            if (count % 1000 == 0)
            {
                Thread.Sleep(100);
            }
            mapControl.Pins.Add(new Pin(mapControl)
            {
                Position = new Position(station.Lat.Value, station.Lon.Value),
                Type = PinType.Svg,
                Svg = @"<svg width=""20"" height=""80"" class=""bg""><circle cx=""10"" cy=""70"" r=""7"" stroke=""Black"" stroke-width=""2"" fill=""White""></circle></svg>",
                Label = "駅名"
            });
        }
    }
    private void drawRoute()
    {
        foreach(var route in VM.routes)
        {
            var paths=route.Paths.OrderBy(r=> r.seq).ToList();
            if (paths.Count == 0)
            {
                continue;
            }

            var p = new Polyline
            {
                StrokeColor = Color.FromRgba( route.color.Value.R, route.color.Value.G, route.color.Value.B, route.color.Value.A),
                StrokeWidth = 6,
                ZIndex = 1
            };
            foreach(var path in paths)
            {
                p.Positions.Add(new Position(path.startStation.Lat.Value, path.startStation.Lon.Value));
            }
            var endStation = paths.Last().endStation;
            p.Positions.Add(new Position(endStation.Lat.Value, endStation.Lon.Value));
             (mapControl).Drawables.Add(p);

        }


    }
}

public class RouteEditFromMapPageModel : INotifyPropertyChanged
{

    public event PropertyChangedEventHandler PropertyChanged;
    public virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
      => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));



    private readonly ObservableCollection<Route> _routes;
    public ObservableCollection<Route> routes { get { return _routes; } }

    private readonly ObservableCollection<Station> _stations;
    public ObservableCollection<Station> stations { get { return _stations; } }



    public RouteEditFromMapPageModel(ICollection<Route> routes, ICollection<Station>stations)
    {
        _routes = routes.ToObservableCollection();
        _stations = stations.ToObservableCollection();

        _routes.CollectionChanged += 
            (object sender, NotifyCollectionChangedEventArgs e)=> { PropertyChanged(this, new PropertyChangedEventArgs("routes")); };
        _stations.CollectionChanged +=
            (object sender, NotifyCollectionChangedEventArgs e) => { PropertyChanged(this, new PropertyChangedEventArgs("stations")); };
    }



}

