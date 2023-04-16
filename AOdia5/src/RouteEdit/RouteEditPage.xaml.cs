using AOdia5.Resources.l18n;
using AOdiaData;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Views;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Path = AOdiaData.Path;

namespace AOdia5;
[QueryProperty(nameof(routeID), "routeID")]
public partial class RouteEditPage : ContentPage, KeyEventListener
{
    private long _s = 0;
    public long routeID
    {
        get { return _s; }
        set
        {
            _s = value;
            loadRoute(_s);
        }
    }


    public RouteEditPage()
    {
        this.BindingContext = this;
        InitializeComponent();
        nameEdit.ValueCheck = (value) =>
        {
            return value.Length > 0;
        };
        nameEdit.OnUnFocused = (value) =>
        {
            if (value == null)
            {
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                var toast = Toast.Make("òHê¸ñºÇãÛÇ…Ç∑ÇÈÇ±Ç∆ÇÕÇ≈Ç´Ç‹ÇπÇÒÅB", ToastDuration.Short, 14);
                toast.Show(cancellationTokenSource.Token);
                return;
            }
            setRouteName(value);
        };
        colorEdit.ValueCheck = (value) =>
        {
            System.Drawing.Color? color = Str2Color(value);
            return color != null;
        };
        colorEdit.OnUnFocused = (value) =>
        {
            if (value == null)
            {
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                var toast = Toast.Make("àŸèÌÇ»êFÉRÅ[ÉhÇ≈Ç∑ÅB", ToastDuration.Short, 14);
                toast.Show(cancellationTokenSource.Token);
                return;
            }
            else
            {
                System.Drawing.Color? color = Str2Color(value);
                if (color != null)
                {
                    setRouteColor((System.Drawing.Color)color);
                }
            }

        };
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
    }
    private async void DeleteStation(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Path path)
        {
            if (route.Paths.Count <= 1)
            {
                if (!await DisplayAlert("âwÇÃçÌèú", "âwÇ™1Ç¬ÇæÇØÇÃòHê¸ÇçÏÇÈÇ±Ç∆ÇÕÇ≈Ç´Ç‹ÇπÇÒÅBécÇËÇÃâwÇ‡çÌèúÇµÇƒéËó«Ç¢Ç≈Ç∑Ç©ÅH", "YES", "Cancel"))
                {
                    return;
                }
            }
            //DeleteStation(path);
        }
    }

    private async void DeleteEndStation(object sender, EventArgs e)
    {
        if (route.Paths.Count <= 1)
        {
            if (!await DisplayAlert("âwÇÃçÌèú", "âwÇ™1Ç¬ÇæÇØÇÃòHê¸ÇçÏÇÈÇ±Ç∆ÇÕÇ≈Ç´Ç‹ÇπÇÒÅBécÇËÇÃâwÇ‡çÌèúÇµÇƒéËó«Ç¢Ç≈Ç∑Ç©ÅH", "YES", "Cancel"))
            {
                return;
            }
        }
//        DeleteEndStation();

    }

    public void OnKeyPress(AOdiaKey keyCode, ModifierKey modifierKey)
    {
        if (keyCode == AOdiaKey.Enter)
        {
            Debug.WriteLine("Enter");
        }
        if (modifierKey.AltPressed && keyCode == AOdiaKey.Left)
        {
            Debug.WriteLine("ñﬂÇÈ");
            (Application.Current.MainPage as MainPage).Back();
        }
    }

    private async void AddStation(object sender, TappedEventArgs e)
    {
        if(sender is View view && view.BindingContext is VMRouteStation station && stationCollectionView.ItemsSource is List<VMRouteStation> stations)
        {
            int stationIndex=stations.IndexOf(station);
            
            string action = await DisplayActionSheet(L18N.ADD_STATION_MODAL_TITLE, "Cancel", null, L18N.ADD_STATION_FROM_LIST, L18N.ADD_STATION_FROM_MAP);
            Action<Station> onStationSelected = (Station station) =>
            {
                UndoCommand addStationCommand = new UndoCommand();
                addStationCommand.Invoke = () =>
                {
                    if (stationIndex == 0)
                    {
                        route.AddStationTop(station);
                    }
                    else if (stationIndex-1 < route.Paths.Count())
                    {
                        route.AddStation(route.Paths.Where(p => p.seq == stationIndex - 1).First(), station);
                    }
                    else
                    {
                        route.AddStationEnd(station);
                    }
                    OnPropertyChanged(nameof(this.Stations));
                };
                addStationCommand.Undo = () =>
                {
                    if(stationIndex<route.Paths.Count())
                    {
                        route.DeleteStation(route.Paths.Where(p => p.seq == stationIndex).First());
                    }
                    else
                    {
                        route.DeleteEndStation();
                    }
                    OnPropertyChanged(nameof(this.Stations));

                };
                addStationCommand.Redo = () =>
                {
                    if (stationIndex == 0)
                    {
                        route.AddStationTop(station);
                    }

                    else if (stationIndex-1 < route.Paths.Count())
                    {
                        route.AddStation(route.Paths.Where(p => p.seq == stationIndex - 1).First(), station);
                    }
                    else
                    {
                        route.AddStationEnd(station);
                    }
                    OnPropertyChanged(nameof(this.Stations));


                };
                UndoStack.Instance.Push(addStationCommand);
            };

            if (action == L18N.ADD_STATION_FROM_LIST)
            {
                if (DeviceInfo.Idiom == DeviceIdiom.Phone)
                {
                    var modal = new StationSelectModal(onStationSelected);
                    await this.Navigation.PushModalAsync(modal);
                }
                else
                {
                    var modal = new StationSelectPopup(this, onStationSelected);
                    this.ShowPopup(modal);

                }

            }
            if (action == L18N.ADD_STATION_FROM_MAP)
            {
                RouteEditFromMapPageModel vm = new RouteEditFromMapPageModel(DiaFile.staticDia.routes.ToList(), DiaFile.staticDia.stations.ToList(), route, null);
                vm.lastStation = route.Paths.OrderBy(p => p.seq).Last().endStation;
                await Navigation.PushAsync(new RouteEditFromMapPage(vm));
            }

        }

    }


    private Route _route;
    public void loadRoute(long id)
    {
        try
        {

        Route? route = DiaFile.staticDia.routes.Include(r => r.Paths).Where(r => r.RouteId == id).FirstOrDefault();
        if (route == null)
        {
            throw new Exception("route is null");
        }
        this._route = route;
        OnPropertyChanged("");
        }catch(Exception ex)
        {
            Debug.WriteLine(ex);
        }

    }
    public Route route { get { 
            if (_route == null) { 
                return new Route(); 
            }
            return _route; } }
    public string routeName
    {
        get
        {
            if (_route == null)
            {
                return "";
            }
            return _route.Name;
        }
    }
    public string routeColorStr
    {
        get
        {
            if (_route == null)
            {
                return "";
            }
            string s = String.Format("{0:X4}", _route.color.ToArgb());
            return new($"#{s.Substring(2)}{s.Substring(0, 2)}");
        }
    }
    public Color routeColor
    {
        get
        {
            if (_route == null)
            {
                return Colors.Black;
            }
            return Color.FromRgba(_route.color.R, _route.color.G, _route.color.B, _route.color.A);
        }
    }
    /*
 * ó^Ç¶ÇÁÇÍÇΩï∂éöóÒÇColorÇ…ïœä∑ÇµÇ‹Ç∑ÅBïœä∑Ç≈Ç´Ç»Ç¢èÍçáÇÕnullÇ™ï‘ÇËÇ‹Ç∑
 */
    public System.Drawing.Color? Str2Color(string colorStr)
    {
        if (colorStr.StartsWith("#"))
        {
            colorStr = colorStr.Substring(1);
        }
        switch (colorStr.Length)
        {
            case 3:
                colorStr = $"{colorStr[0]}{colorStr[0]}{colorStr[1]}{colorStr[1]}{colorStr[2]}{colorStr[2]}FF";
                break;
            case 4:
                colorStr = $"{colorStr[0]}{colorStr[0]}{colorStr[1]}{colorStr[1]}{colorStr[2]}{colorStr[2]}{colorStr[3]}{colorStr[3]}";
                break;
            case 6:
                colorStr = colorStr + "FF";
                break;
            case 8:
                break;
            default:
                return null;
        }
        try
        {

            int a = Convert.ToInt32(colorStr.Substring(6, 2), 16);
            int r = Convert.ToInt32(colorStr.Substring(0, 2), 16);
            int g = Convert.ToInt32(colorStr.Substring(2, 2), 16);
            int b = Convert.ToInt32(colorStr.Substring(4, 2), 16);

            return System.Drawing.Color.FromArgb(a, r, g, b);
        }
        catch (Exception)
        {
            return null;
        }

    }


    public List<VMRouteStation> Stations
    {
        get
        {
            var result = route.Paths.OrderBy(r => r.seq).Select(r => new VMRouteStation(r.startStation)).ToList();
            if(result.Count > 0) { 
            result.Add(new VMRouteStation(route.Paths.OrderBy(r => r.seq).Last().endStation));
            }
            result.Insert(0,new VMRouteStation(null));
            return result;
        }
    }
    public void setRouteName(string newRouteName)
    {
        string oldRouteName = _route.Name;
        if (oldRouteName == newRouteName) return;
        UndoCommand routeNameChange = new UndoCommand();
        routeNameChange.comment = $"RouteNameChange";
        routeNameChange.Invoke = () =>
        {
            _route.Name = newRouteName;
            DiaFile.staticDia.SaveChanges();
        };

        routeNameChange.Undo = () =>
        {
            _route.Name = oldRouteName;
            DiaFile.staticDia.SaveChanges();
        };

        routeNameChange.Redo = () =>
        {
            _route.Name = newRouteName;
            DiaFile.staticDia.SaveChanges();
        };
        UndoStack.Instance.Push(routeNameChange);


    }
    public void setRouteColor(System.Drawing.Color newColor)
    {
        System.Drawing.Color oldColor = _route.color;
        if (oldColor.Equals(newColor)) return;
        UndoCommand routeNameChange = new UndoCommand();
        routeNameChange.comment = $"RouteColorChange";
        routeNameChange.Invoke = () =>
        {
            _route.color = newColor;
            DiaFile.staticDia.SaveChanges();
        };

        routeNameChange.Undo = () =>
        {
            _route.color = oldColor;
            DiaFile.staticDia.SaveChanges();
        };

        routeNameChange.Redo = () =>
        {
            _route.color = newColor;
            DiaFile.staticDia.SaveChanges();
        };
        UndoStack.Instance.Push(routeNameChange);
    }




}




public class StationSelectPopup : Popup
{
    Action<Station>? onStationSelected;

    public StationSelectPopup(Page page,Action<Station> onStationSelected)
    {
        this.onStationSelected = onStationSelected;
        var main = new StationSelectorView();
        this.Content = main;
        this.Size = new Size(page.Width-100, page.Height - 100);
        this.Color = Color.FromRgb(240, 240, 240);

        main.OnStationSelected += Main_OnStationSelected;
    }

    private async void Main_OnStationSelected(object? sender, EventArgs e)
    {
        if (e is OnStationSelectedEventArgs args && this.onStationSelected != null)
        {
            this.Close();
            this.onStationSelected(args.station);
            this.onStationSelected = null;
        }
    }

    private async void CloseButton_Clicked(object sender, EventArgs e)
    {
    }
}
public class StationSelectModal : ContentPage
{
    Action<Station> onStationSelected;
    public StationSelectModal(Action<Station> onStationSelected)
    {
        this.onStationSelected = onStationSelected;
        var main = new StationSelectorView();
        this.Content = main;

        main.OnStationSelected += OnStationSelected;
    }

    private void OnStationSelected(object? sender, EventArgs e)
    {
        if (e is OnStationSelectedEventArgs args)
        {

            onStationSelected(args.station);
            this.Navigation.PopModalAsync();
        }
    }

    private async void CloseButton_Clicked(object sender, EventArgs e)
    {
    }

}

public class VMRouteStation
{
    public Station? station;
    public VMRouteStation(Station? station)
    {
        this.station = station;
    }
    public bool isNotNull
    {
        get
        {
            return station != null ;
        }
    }

    public String name { get { return station?.DbName ?? ""; } }
    public String lat { get { return station?.DbLat.ToString("F4") ?? ""; } }
    public String lon { get { return station?.DbLon.ToString("F4") ?? ""; } }
    public String routes
    {
        get
        {
            if (station == null)
            {
                return "";
            }
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

