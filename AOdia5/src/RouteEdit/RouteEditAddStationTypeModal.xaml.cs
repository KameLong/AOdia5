using AOdiaData;
using CommunityToolkit.Maui.Views;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Reactive.Bindings;

namespace AOdia5;
using Path = AOdiaData.Path;

public partial class RouteEditAddStationTypeModal : Popup
{
    private readonly ReactiveProperty<Route> _route;
    private readonly Path _path;
    private readonly INavigation _nav;
	public RouteEditAddStationTypeModal(ReactiveProperty<Route> route,Path path,INavigation nav)
	{
        _route = route;
        _path = path;
        _nav = nav;

		InitializeComponent();
	}

    private void AddStationFromMapClicked(object sender, EventArgs e)
    {
        RouteEditFromMapPageModel VM = new RouteEditFromMapPageModel(DiaFile.staticDia.routes.ToList(),DiaFile.staticDia.stations.ToList(),_route.Value,_path);
        _nav.PushAsync(new RouteEditFromMapPage(VM));

    }

    private void AddStationFromListClicked(object sender, EventArgs e)
    {

    }
}