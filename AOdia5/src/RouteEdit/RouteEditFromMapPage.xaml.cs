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
        DrawRoute();
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
            AddStation(station);
        }
    }
    public void AddStation(in Station station)
    {
        mapControl.Pins.Add(new Pin(mapControl)
        {

        Position = new Position(station.Lat.Value, station.Lon.Value),
            Type = PinType.Svg,
            Svg = @"<svg width=""20"" height=""80"" class=""bg""><circle cx=""10"" cy=""70"" r=""7"" stroke=""Black"" stroke-width=""2"" fill=""White""></circle></svg>",
            Label = "駅名"
        });

    }
    private void DrawRoute()
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
                StrokeColor = Color.FromRgba( route.color.R, route.color.G, route.color.B, route.color.A),
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
    private void DrawRoute(Route route)
    {
        (mapControl).Drawables.Clear();
        var paths = route.Paths.OrderBy(r => r.seq).ToList();
        if (paths.Count == 0)
        {
            return;
        }

        var p = new Polyline
        {
            StrokeColor = Color.FromRgba(route.color.R, route.color.G, route.color.B, route.color.A),
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
    private async void MapClicked(object? sender, MapClickedEventArgs e)
    {
        string? name = await DisplayPromptAsync("StationName", "Write name of the station.");
        if(name == null)
        {
            //キャンセル
            return;
        }
        selectedpos = e.Point;
        var station=VM.AddNewStation((float)selectedpos.Value.Latitude, (float)selectedpos.Value.Longitude);
        station.Name.Value = name ?? "デフォルト駅名";
        AddStation(station);
        DrawRoute(VM.route);



    }

}

public class RouteEditFromMapPageModel : INotifyPropertyChanged
{

    public event PropertyChangedEventHandler? PropertyChanged;
    public virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
      => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));



    private readonly ObservableCollection<Route> _routes;
    public ObservableCollection<Route> routes { get { return _routes; } }

    private readonly ObservableCollection<Station> _stations;
    public ObservableCollection<Station> stations { get { return _stations; } }

    public Route route;
    public Path? editPath;
    public Station? lastStation;




    public RouteEditFromMapPageModel(ICollection<Route> routes, ICollection<Station>stations,Route r,Path? p)
    {
        route = r;
        editPath = p;
        _routes = routes.ToObservableCollection();
        _stations = stations.ToObservableCollection();
    }

    public Station AddNewStation(float lat,float lon)
    {
        var station = Station.CreateNewStation();
        station.DbName = "test";
        station.DbLat = lat;
        station.DbLon = lon;
        stations.Add(station);
        if (editPath != null)
        {
            editPath = route.AddStation(editPath, station);
        }
        else
        {
            if(lastStation != null)
            {
                //末尾追加
                route.AddPath(lastStation,station);
                lastStation = station;

            }
            else
            {
                //先頭追加
                if(route.Paths.Count > 0)
                {
                    editPath = route.AddStationTop(station);
                }
                else
                {
                    lastStation = station;

                }
            }
        }

        DiaFile.staticDia.SaveChanges();
        return station;

    }



}

