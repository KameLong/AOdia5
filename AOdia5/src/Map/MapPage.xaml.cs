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

        pos= new Position[VM.stations.Count];
        int i = 0;
        foreach(var station in VM.stations){
            pos[i]= new Position(station.Lat.Value,station.Lon.Value);
            i++;
        }
        Init();

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
        foreach (var po in pos)
        {
            mapControl.Pins.Add(new Pin(mapControl)
            {
                Position = po,
                Type = PinType.Svg,
                Svg = @"<svg width=""20"" height=""80"" class=""bg""><circle cx=""10"" cy=""70"" r=""7"" stroke=""Black"" stroke-width=""2"" fill=""White""></circle></svg>",
                Label = "駅名"
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
        (mapControl).Drawables.Add(p);
    }

    private void MapClicked(object sender, MapClickedEventArgs e)
    {
        selectedpos = e.Point;
        mapControl.Pins.Add(new Pin(mapControl){ Position= (Position)selectedpos,Label="station"});
        selectModal.ZIndex = 10;
    }

    //新規駅を追加します。
    private void CreateNewStation(object sender, EventArgs e)
    {
        CloseSeletModal();
    }

    //既存駅に場所を設定します。
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
        StaticData.staticDia.SaveChanges();
        Navigation.PopAsync();
    }
}