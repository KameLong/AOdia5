using AOdia5.Resources.l18n;
using AOdiaData;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Extensions;
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
            VM.loadRoute(_s);
        }
    }

    private RouteEditPageModel VM { get { return BindingContext as RouteEditPageModel; } set { BindingContext = value; } }

    public RouteEditPage()
    {
        VM = new RouteEditPageModel();
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
            VM.setRouteName(value);
        };
        colorEdit.ValueCheck = (value) =>
        {
            System.Drawing.Color? color = VM.Str2Color(value);
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
                System.Drawing.Color? color = VM.Str2Color(value);
                if (color != null)
                {
                    VM.setRouteColor((System.Drawing.Color)color);
                }
            }

        };
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        VM.onLoad();
    }
    //private async void AddStation(object sender, EventArgs e)
    //{
    //    if (sender is Button button && button.BindingContext is Path path)
    //    {
    //        //stationÇÃå„Ç…óÒé‘Çí«â¡Ç∑ÇÈ   
    //        string action = await DisplayActionSheet("", "Cancel", null, L18N.ADD_STATION_FROM_LIST, L18N.ADD_STATION_FROM_MAP);
    //        if (action == L18N.ADD_STATION_FROM_LIST)
    //        {

    //        }
    //        if (action == L18N.ADD_STATION_FROM_MAP)
    //        {
    //            RouteEditFromMapPageModel vm = new RouteEditFromMapPageModel(DiaFile.staticDia.routes.ToList(), DiaFile.staticDia.stations.ToList(), VM.route, path);
    //            await Navigation.PushAsync(new RouteEditFromMapPage(vm));
    //        }
    //    }
    //    else if (sender is Button)
    //    {
    //        //ç≈èâÇÃâwÇí«â¡Ç∑ÇÈ
    //        string action = await DisplayActionSheet("", "Cancel", null, L18N.ADD_STATION_FROM_LIST, L18N.ADD_STATION_FROM_MAP);
    //        if (action == L18N.ADD_STATION_FROM_LIST)
    //        {

    //        }
    //        if (action == L18N.ADD_STATION_FROM_MAP)
    //        {
    //            RouteEditFromMapPageModel vm = new RouteEditFromMapPageModel(DiaFile.staticDia.routes.ToList(), DiaFile.staticDia.stations.ToList(), VM.route, null);
    //            await Navigation.PushAsync(new RouteEditFromMapPage(vm));
    //        }
    //    }
    //}


    private async void DeleteStation(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Path path)
        {
            if (VM.route.Paths.Count <= 1)
            {
                if (!await DisplayAlert("âwÇÃçÌèú", "âwÇ™1Ç¬ÇæÇØÇÃòHê¸ÇçÏÇÈÇ±Ç∆ÇÕÇ≈Ç´Ç‹ÇπÇÒÅBécÇËÇÃâwÇ‡çÌèúÇµÇƒéËó«Ç¢Ç≈Ç∑Ç©ÅH", "YES", "Cancel"))
                {
                    return;
                }
            }
            VM.DeleteStation(path);
        }
    }

    private async void DeleteEndStation(object sender, EventArgs e)
    {
        if (VM.route.Paths.Count <= 1)
        {
            if (!await DisplayAlert("âwÇÃçÌèú", "âwÇ™1Ç¬ÇæÇØÇÃòHê¸ÇçÏÇÈÇ±Ç∆ÇÕÇ≈Ç´Ç‹ÇπÇÒÅBécÇËÇÃâwÇ‡çÌèúÇµÇƒéËó«Ç¢Ç≈Ç∑Ç©ÅH", "YES", "Cancel"))
            {
                return;
            }
        }
        VM.DeleteEndStation();

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
    public bool Test(string str)
    {
        return true;
    }

    private async void AddStation(object sender, TappedEventArgs e)
    {
        string action = await DisplayActionSheet(L18N.ADD_STATION_MODAL_TITLE, "Cancel", null, L18N.ADD_STATION_FROM_LIST, L18N.ADD_STATION_FROM_MAP);
        if (action == L18N.ADD_STATION_FROM_LIST)
        {

        }
        if (action == L18N.ADD_STATION_FROM_MAP)
        {
            RouteEditFromMapPageModel vm = new RouteEditFromMapPageModel(DiaFile.staticDia.routes.ToList(), DiaFile.staticDia.stations.ToList(), VM.route, null);
            vm.lastStation = VM.route.Paths.OrderBy(p => p.seq).Last().endStation;
            await Navigation.PushAsync(new RouteEditFromMapPage(vm));
        }
    }
}

public class RouteEditPageModel : Bindable
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


    private Route _route;
    public void loadRoute(long id)
    {
        Route? route = DiaFile.staticDia.routes.Include(r => r.Paths).Where(r => r.RouteId == id).FirstOrDefault();
        if (route == null)
        {
            throw new Exception("route is null");
        }
        this._route = route;
        OnPropertyChanged("");
    }
    public Route route { get { return _route; } }

    public string routeName { get {
            if (_route == null)
            {
                return "";
            }
            return _route.Name; } }
    public string routeColorStr { get {
            if (_route == null) {
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


    public List<VMStation> Stations { get {
            var result = route.Paths.OrderBy(r => r.seq).Select(r => new VMStation(r.startStation)).ToList();
            result.Add(new VMStation(route.Paths.OrderBy(r => r.seq).Last().endStation));
            return result;
        }
    }

    public ObservableCollection<Path> Paths { get { return _route.Paths.OrderBy(p => p.seq).ToObservableCollection(); } }
    public Station? endStation { get { if (Paths.Count > 0) { return Paths.Last().endStation; } else { return null; } } }
    public bool EndStationIsNotNull { get { return endStation != null; } }

    private RouteListPageModel? routeListPageModel { get; set; }


    /*
     * ó^Ç¶ÇÁÇÍÇΩï∂éöóÒÇColorÇ…ïœä∑ÇµÇ‹Ç∑ÅBïœä∑Ç≈Ç´Ç»Ç¢èÍçáÇÕnullÇ™ï‘ÇËÇ‹Ç∑
     */
    public System.Drawing.Color? Str2Color(string colorStr)
    {
        if (colorStr.StartsWith("#"))
        {
            colorStr=colorStr.Substring(1);
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
                    colorStr= colorStr + "FF";
                    break;
                case 8:
                    break;
                default:
                return null;
            }
        try
        {

        int a=Convert.ToInt32(colorStr.Substring(6,2),16);
        int r = Convert.ToInt32(colorStr.Substring(0, 2),16);
        int g = Convert.ToInt32(colorStr.Substring(2, 2),16);
        int b = Convert.ToInt32(colorStr.Substring(4, 2),16);

            return System.Drawing.Color.FromArgb(a,r,g,b);
                }catch (Exception) {
            return null;
        }

        }


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
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Paths)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(endStation)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(EndStationIsNotNull)));
        }
    }
    /*
        * pathÇÃstartStationÇçÌèúÇµÇ‹Ç∑ÅB
        */
    public void DeleteStation(Path path)
    {
        UndoCommand deleteStationCommand = new UndoCommand();
        deleteStationCommand.comment = $"DeletePath({path.pathID})";
        deleteStationCommand.Invoke = () =>
        {
            _route.DeleteStation(path);
            DiaFile.staticDia.SaveChanges();
        };

        deleteStationCommand.Undo = () =>
        {
            _route.InsertStation(path);
            DiaFile.staticDia.SaveChanges();
        };

        deleteStationCommand.Redo = () =>
        {
            _route.DeleteStation(path);
            DiaFile.staticDia.SaveChanges();
        };
        UndoStack.Instance.Push(deleteStationCommand);

    }
    /*
     * pathÇÃstartStationÇçÌèúÇµÇ‹Ç∑ÅB
     */
    public void DeleteEndStation()
    {
        if(_route.Paths.Count == 1)
        {
            DeleteStation(_route.Paths.First());
            return;
        }

        UndoCommand deleteEndStationCommand = new UndoCommand();
        Station deleteStation = _route.Paths.OrderBy(p => p.seq).Last().endStation;
        deleteEndStationCommand.comment = $"DeleteEndPath";
        deleteEndStationCommand.Invoke = () =>
        {
            _route.DeleteEndStation();
            DiaFile.staticDia.SaveChanges();
        };

        deleteEndStationCommand.Undo = () =>
        {
            _route.AddStationEnd(deleteStation);
            DiaFile.staticDia.SaveChanges();
        };

        deleteEndStationCommand.Redo = () =>
        {
            _route.DeleteEndStation();
            DiaFile.staticDia.SaveChanges();
        };
        UndoStack.Instance.Push(deleteEndStationCommand);

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

public class VMRouteStation
{
    public string Name { get;  }
}