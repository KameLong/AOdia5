using AOdia5.Resources.l18n;
using AOdiaData;
using CommunityToolkit.Maui.Core.Extensions;
using Reactive.Bindings;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Path = AOdiaData.Path;

namespace AOdia5;

public partial class RouteEditPage : ContentPage
{

    private RouteEditPageModel VM { get { return BindingContext as RouteEditPageModel; } set { BindingContext = value; } }
    public RouteEditPage(RouteEditPageModel vm) : this()
    {
        VM = vm;
    }

    public RouteEditPage()
	{
		InitializeComponent();
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        VM.onLoad();
    }



    private async void  AddStation(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Path path)
        {
            //station‚ÌŒã‚É—ñŽÔ‚ð’Ç‰Á‚·‚é   
            string action = await DisplayActionSheet("", "Cancel", null, L18N.ADD_STATION_FROM_LIST, L18N.ADD_STATION_FROM_MAP);
            if(action== L18N.ADD_STATION_FROM_LIST)
            {

            }
            if(action== L18N.ADD_STATION_FROM_MAP)
            {
                RouteEditFromMapPageModel vm = new RouteEditFromMapPageModel(DiaFile.staticDia.routes.ToList(), DiaFile.staticDia.stations.ToList(), VM.route.Value, path);
                await Navigation.PushAsync(new RouteEditFromMapPage(vm));

            }
 
        }
        else if(sender is Button )
        {
            //Å‰‚Ì‰w‚ð’Ç‰Á‚·‚é
            string action = await DisplayActionSheet("", "Cancel", null, L18N.ADD_STATION_FROM_LIST, L18N.ADD_STATION_FROM_MAP);
            if (action == L18N.ADD_STATION_FROM_LIST)
            {

            }
            if (action == L18N.ADD_STATION_FROM_MAP)
            {
                RouteEditFromMapPageModel vm = new RouteEditFromMapPageModel(DiaFile.staticDia.routes.ToList(), DiaFile.staticDia.stations.ToList(), VM.route.Value,null);
                await Navigation.PushAsync(new RouteEditFromMapPage(vm));
            }
        }

    }

    private async void Button_Clicked(object sender, EventArgs e)
    {

        string action = await DisplayActionSheet(L18N.ADD_STATION_MODAL_TITLE, "Cancel", null, L18N.ADD_STATION_FROM_LIST, L18N.ADD_STATION_FROM_MAP);
        if (action == L18N.ADD_STATION_FROM_LIST)
        {

        }
        if (action == L18N.ADD_STATION_FROM_MAP)
        {
            RouteEditFromMapPageModel vm = new RouteEditFromMapPageModel(DiaFile.staticDia.routes.ToList(), DiaFile.staticDia.stations.ToList(), VM.route.Value, null);
            vm.lastStation= VM.route.Value.Paths.OrderBy(p => p.seq).Last().endStation;
            await Navigation.PushAsync(new RouteEditFromMapPage(vm));
        }


    }



}


public class RouteEditPageModel : INotifyPropertyChanged
{

    public event PropertyChangedEventHandler PropertyChanged;
    public virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
      => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


    private readonly Route _route;

    public ReactiveProperty<Route> route { get { return new ReactiveProperty<Route>(_route); } }


    public ObservableCollection<Path> Paths { get { return _route.Paths.OrderBy(p => p.seq).ToObservableCollection();  } }

    public Station? endStation { get { if (Paths.Count > 0) { return Paths.Last().endStation; } else { return null; } } }
    public bool EndStationIsNotNull { get { return endStation != null; } }

    private RouteListPageModel? routeListPageModel { get; set; }

    public string routeColorHtml {
        get {
            string s = String.Format("{0:X4}", _route.color.Value.ToArgb());

            return new ($"#{s.Substring(2)}{s.Substring(0,2)}");
        }set {
            if (value.Length > 1) {
                var color="";
                switch(value.Length)
                {
                    case 4:
                        color = $"#{value[1]}{value[1]}{value[2]}{value[2]}{value[3]}{value[3]}FF";
                        break;
                    case 5:
                        color = $"#{value[1]}{value[1]}{value[2]}{value[2]}{value[3]}{value[3]}{value[4]}{value[4]}";
                        break;
                    case 7:
                        color = value + "FF";
                        break;
                    case 9:
                        color = value;
                        break;
                    default:
                        color ="#000000FF";
                        break;
                }

                _route.color.Value = System.Drawing.Color.FromArgb(Convert.ToInt32(color.Substring(7, 2), 16), Convert.ToInt32(color.Substring(1, 2), 16), Convert.ToInt32(color.Substring(3, 2), 16), Convert.ToInt32(color.Substring(5, 2), 16));
            }
        
        } }

    public RouteEditPageModel(Route route, RouteListPageModel routeListPageModel)
    {
        _route = route;
        this.routeListPageModel = routeListPageModel;


    }
    public RouteEditPageModel()
    {
        _route = new Route();
    }




    public void onLoad()
    {
        PropertyChanged(this, new PropertyChangedEventArgs("Paths"));
        PropertyChanged(this, new PropertyChangedEventArgs("endStation"));
        PropertyChanged(this, new PropertyChangedEventArgs("EndStationIsNotNull"));
    }






}

