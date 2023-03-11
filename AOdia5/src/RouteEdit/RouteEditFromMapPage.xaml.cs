using AOdiaData;
using CommunityToolkit.Maui.Core.Extensions;
using Mapsui.UI.Maui;
using Microsoft.EntityFrameworkCore;
using Reactive.Bindings;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using Path = AOdiaData.Path;

namespace AOdia5;

public partial class RouteEditFromMapPage : ContentPage
{
    private Position? selectedpos = null;

    public RouteEditFromMapPageModel VM { get { return BindingContext as RouteEditFromMapPageModel; } set { BindingContext = value; } }
	public RouteEditFromMapPage(RouteEditFromMapPageModel vm)
	{
       this.VM = vm;
		InitializeComponent();
       mapControl.Map?.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer());
       mapControl.MapClicked += MapClicked;




        drawStation();
        drawRoute();
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
                Label = "‰w–¼"
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
    private void drawRoute(Route route)
    {
        var paths = route.Paths.OrderBy(r => r.seq).ToList();
        if (paths.Count == 0)
        {
            return;
        }

        var p = new Polyline
        {
            StrokeColor = Color.FromRgba(route.color.Value.R, route.color.Value.G, route.color.Value.B, route.color.Value.A),
            StrokeWidth = 6,
            ZIndex = 1
        };
        foreach (var path in paths)
        {
            p.Positions.Add(new Position(path.startStation.Lat.Value, path.startStation.Lon.Value));
        }
        var endStation = paths.Last().endStation;
        p.Positions.Add(new Position(endStation.Lat.Value, endStation.Lon.Value));
        (mapControl).Drawables.Add(p);

    }
    private void MapClicked(object sender, MapClickedEventArgs e)
    {
        selectedpos = e.Point;
        var station = new Station {DbName="test",DbLat=(float)selectedpos.Value.Latitude,DbLon=(float) selectedpos.Value.Longitude };
        DiaFile.staticDia.stations.Add(station);
        VM.stations.Add(station);
        DiaFile.staticDia.SaveChanges();
        mapControl.Pins.Add(new Pin(mapControl)
        {
            Position = new Position(station.Lat.Value, station.Lon.Value),
            Type = PinType.Svg,
            Svg = @"<svg width=""20"" height=""80"" class=""bg""><circle cx=""10"" cy=""70"" r=""7"" stroke=""Black"" stroke-width=""2"" fill=""White""></circle></svg>",
            Label = "‰w–¼"
        });
        var newPath = new Path();
        newPath.route = VM.route;
        newPath.startStation = station;
        newPath.endStation = VM.editPath.endStation;
        newPath.seq = VM.editPath.seq + 1;
        VM.route.Paths.Where(p => p.seq > VM.editPath.seq).Select(p => { p.seq++;return true; });
        VM.editPath.endStation = station;
        
        VM.route.Paths.Add(newPath);
        DiaFile.staticDia.paths.Add(newPath);
        VM.editPath = newPath;

        DiaFile.staticDia.SaveChanges();
        drawRoute(VM.route);


        //        Navigation.PopAsync();

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

    public Route route;
    public Path editPath;



    public RouteEditFromMapPageModel(ICollection<Route> routes, ICollection<Station>stations,Route r,Path p)
    {
        route = r;
        editPath = p;
        _routes = routes.ToObservableCollection();
        _stations = stations.ToObservableCollection();

        //_routes.CollectionChanged += 
        //    (object sender, NotifyCollectionChangedEventArgs e)=> { PropertyChanged(this, new PropertyChangedEventArgs("routes")); };
        //_stations.CollectionChanged +=
        //    (object sender, NotifyCollectionChangedEventArgs e) => { PropertyChanged(this, new PropertyChangedEventArgs("stations")); };
    }



}

