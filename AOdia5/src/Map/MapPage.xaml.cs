using AOdiaData;
using Mapsui.UI.Maui;
using System.Diagnostics;
using System.Net.Sockets;

namespace AOdia5;
public partial class MapPage : ContentPage
   
{
    private Position[] pos = new Position[0];


    private Position? selectedpos = null;
    internal MapPage(MapViewModel viewModel):this()
    {
        this.BindingContext = viewModel;

        pos= new Position[viewModel.stations.Count];
        int i = 0;
        foreach(var station in viewModel.stations){
            pos[i]= new Position(station.Lat.Value,station.Lon.Value);
            i++;
        }
        Init();

    }
	public MapPage()
	{

		InitializeComponent();
        this.BindingContext = new MapViewModel();

        mapControl.Map?.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer());
        mapControl.MapClicked += MapControl_MapClicked;

        
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
                Svg = "<svg width=\"20\" height=\"80\" class=\"bg\">\r\n    <circle cx=\"10\" cy=\"70\" r=\"7\" stroke=\"Black\" stroke-width=\"2\" fill=\"White\"></circle>\r\n</svg>",
                Label = "âwñº",
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
        //        (mapControl).Drawables.Add(w
    }

    private void MapControl_MapClicked(object sender, MapClickedEventArgs e)
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
        if (((MapViewModel)BindingContext).editStation != null)
        {
            //StaticData.staticDia.ChangeTracker.DetectChanges();
            //Debug.WriteLine("Ç®Ç®Ç®Ç®Ç®Ç©");
            Debug.WriteLine(StaticData.staticDia.ChangeTracker.DebugView.LongView);

            ((MapViewModel)BindingContext).editStation.editStation.Lat.Value = (float)selectedpos.Value.Latitude;
            ((MapViewModel)BindingContext).editStation.editStation.Lon.Value = (float)selectedpos.Value.Longitude;

//            StaticData.staticDia.Stations.Update(((MapViewModel)BindingContext).editStation.editStation);

            StaticData.staticDia.ChangeTracker.DetectChanges();
            Debug.WriteLine("Ç®Ç®Ç®Ç®Ç®Çê");
            Debug.WriteLine(StaticData.staticDia.ChangeTracker.DebugView.LongView);

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