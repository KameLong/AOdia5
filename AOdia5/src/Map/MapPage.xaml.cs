using AOdiaData;
using Mapsui.UI.Maui;
using System.Diagnostics;
using System.Net.Sockets;

namespace AOdia5;
public partial class MapPage : ContentPage
   
{
    private Position[] pos = new Position[0];
    /*
    private Position[] pos = new[]{ new Position(35.20338500073202, 136.929724876968),
                new Position(35.19620247763443, 136.92982101441822),
                new Position(35.188931972323104, 136.92914853019147),
                new Position(35.182424119409234, 136.92314303394488),
                new Position(35.17883669438328, 136.91739460606436),

                new Position(35.17535924977982, 136.91421751789377),
                new Position(35.16976463558611, 136.91452062871312),
                new Position(35.161589574926516, 136.91561694612017),


                new Position(35.15658449014186, 136.91619976679564),
                new Position(35.14971795634483, 136.91732408567094),
                new Position(35.14002959443394, 136.91827961802716),
                new Position(35.130517984169835, 136.91944691561633),
                new Position(35.12100209198829, 136.9210374684769),
                new Position(35.11632946023994, 136.92171988978788),
                new Position(35.108159002977146, 136.9229553580547),
                new Position(35.09577855318187, 136.92657026145088) };
    */


    private Position? selectedpos = null;
    internal MapPage(MapViewModel viewModel):this()
    {
        this.BindingContext = viewModel;

        pos= new Position[viewModel.stations.Count];
        int i = 0;
        foreach(var station in viewModel.stations){
            pos[i]= new Position(station.Lat,station.Lon);
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
            ((MapViewModel)BindingContext).editStation.editStation.Lat = (float)selectedpos.Value.Latitude;
            ((MapViewModel)BindingContext).editStation.editStation.Lon = (float)selectedpos.Value.Longitude;
            StaticData.staticDia.Stations.Update(((MapViewModel)BindingContext).editStation.editStation);
            StaticData.staticDia.SaveChanges();
        }
        CloseSeletModal();
        Application.Current.MainPage = new StationList();

    }

    private void OnClickCloseSelectModal(object sender, TappedEventArgs e)
    {

        CloseSeletModal();

    }
    private void CloseSeletModal() {
        mapControl.Pins.Remove(mapControl.Pins.Last());
        selectModal.ZIndex = -10;
    }
}