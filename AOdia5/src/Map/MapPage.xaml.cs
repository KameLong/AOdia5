
using AOdiaData;
using Mapsui.UI.Maui;
using System.Diagnostics;
using System.Net.Sockets;

namespace AOdia5;
public partial class MapPage : ContentPage
{
    private Position[] pos = new Position[0];
    private Position? selectedpos = null;
    public MapViewModel VM { get { return BindingContext as MapViewModel; } set { BindingContext = value; } }

    internal MapPage(MapViewModel viewModel):this()
    {
        VM = viewModel;
        pos = new Position[VM.stations.Count];

        new Thread(new ThreadStart(() => {
            int i = 0;
            foreach (var station in VM.stations)
            {
                pos[i] = new Position(station.Lat.Value, station.Lon.Value);
                i++;
            }
            Thread.Sleep(1000);
            Init();

        })).Start();
        int a = 0;
    }

	public MapPage()
	{
		InitializeComponent();
        VM = new MapViewModel();
        mapControl.Map?.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer());
        mapControl.MapClicked += MapClicked;
    }

    private void Init()
    {
        mapControl.Pins.Clear();
        int count = 0;
        foreach (var po in pos)
        {
            count++;
            if (count % 1000 == 0)
            {
                Thread.Sleep(100);
            }
            mapControl.Pins.Add(new Pin(mapControl)
            {
                Position = po,
                Type = PinType.Svg,
                Svg = @"<svg width=""20"" height=""80"" class=""bg""><circle cx=""10"" cy=""70"" r=""7"" stroke=""Black"" stroke-width=""2"" fill=""White""></circle></svg>",
                Label = "âwñº"
            });
        }

        var w = new Polyline
        {
            StrokeColor = Colors.White,
            StrokeWidth = 10,
            ZIndex = 0,

        };

        var p = new Polyline
        {
            StrokeColor = Colors.HotPink,
            StrokeWidth = 6,
            ZIndex = 1
        };

        foreach (var po in pos)
        {
            p.Positions.Add(po);
            w.Positions.Add(po);
        }
//        (mapControl).Drawables.Add(p);
    }

    private void MapClicked(object sender, MapClickedEventArgs e)
    {
        selectedpos = e.Point;
        mapControl.Pins.Add(new Pin(mapControl){ Position= (Position)selectedpos,Label="station"});
        selectModal.ZIndex = 10;
    }

    //êVãKâwÇí«â¡ÇµÇ‹Ç∑ÅB
    private void CreateNewStation(object sender, EventArgs e)
    {
        CloseSeletModal();
    }

    //ä˘ë∂âwÇ…èÍèäÇê›íËÇµÇ‹Ç∑ÅB
    private void SetStationPos(object sender, EventArgs e)
    {
        if (VM.editStation != null)
        {
            VM.editStation.editStation.Lat.Value = (float)selectedpos.Value.Latitude;
            VM.editStation.editStation.Lon.Value = (float)selectedpos.Value.Longitude;
        }
        CloseSeletModal();
        ClosePage();
    }

    private void OnClickCloseSelectModal(object sender, TappedEventArgs e)
    {
        CloseSeletModal();
    }
    private void CloseSeletModal() {
        mapControl.Pins.Remove(mapControl.Pins.Last());
        selectModal.ZIndex = -10;
    }
    private void ClosePage()
    {
//        AOdiaData.DiaFile.staticDia.SaveChanges();
        Navigation.PopAsync();
    }
}