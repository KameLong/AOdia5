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
                var toast = Toast.Make("路線名を空にすることはできません。", ToastDuration.Short, 14);
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
                var toast = Toast.Make("異常な色コードです。", ToastDuration.Short, 14);
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
//        VM.onLoad();
    }
    private async void DeleteStation(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Path path)
        {
            if (route.Paths.Count <= 1)
            {
                if (!await DisplayAlert("駅の削除", "駅が1つだけの路線を作ることはできません。残りの駅も削除して手良いですか？", "YES", "Cancel"))
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
            if (!await DisplayAlert("駅の削除", "駅が1つだけの路線を作ることはできません。残りの駅も削除して手良いですか？", "YES", "Cancel"))
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
            Debug.WriteLine("戻る");
            (Application.Current.MainPage as MainPage).Back();
        }
    }


    private async void AddStation(object sender, TappedEventArgs e)
    {
        string action = await DisplayActionSheet(L18N.ADD_STATION_MODAL_TITLE, "Cancel", null, L18N.ADD_STATION_FROM_LIST, L18N.ADD_STATION_FROM_MAP);
        if (action == L18N.ADD_STATION_FROM_LIST)
        {
            var modalPage = new ModalPage(this);

            await Navigation.PushModalAsync(modalPage);
        }
        if (action == L18N.ADD_STATION_FROM_MAP)
        {
            RouteEditFromMapPageModel vm = new RouteEditFromMapPageModel(DiaFile.staticDia.routes.ToList(), DiaFile.staticDia.stations.ToList(), route, null);
            vm.lastStation = route.Paths.OrderBy(p => p.seq).Last().endStation;
            await Navigation.PushAsync(new RouteEditFromMapPage(vm));
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
 * 与えられた文字列をColorに変換します。変換できない場合はnullが返ります
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


    public List<VMStation> Stations
    {
        get
        {
            var result = route.Paths.OrderBy(r => r.seq).Select(r => new VMStation(r.startStation)).ToList();
            if(result.Count > 0) { 
            result.Add(new VMStation(route.Paths.OrderBy(r => r.seq).Last().endStation));
            }
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

//public class RouteEditPageModel : Bindable
//{
//    public event PropertyChangedEventHandler? PropertyChanged;
//    public virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
//        => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

//    ///*
//    //    * pathのstartStationを削除します。
//    //    */
//    //public void DeleteStation(Path path)
//    //{
//    //    UndoCommand deleteStationCommand = new UndoCommand();
//    //    deleteStationCommand.comment = $"DeletePath({path.pathID})";
//    //    deleteStationCommand.Invoke = () =>
//    //    {
//    //        _route.DeleteStation(path);
//    //        DiaFile.staticDia.SaveChanges();
//    //    };

//    //    deleteStationCommand.Undo = () =>
//    //    {
//    //        _route.InsertStation(path);
//    //        DiaFile.staticDia.SaveChanges();
//    //    };

//    //    deleteStationCommand.Redo = () =>
//    //    {
//    //        _route.DeleteStation(path);
//    //        DiaFile.staticDia.SaveChanges();
//    //    };
//    //    UndoStack.Instance.Push(deleteStationCommand);

//    //}
//    ///*
//    // * pathのstartStationを削除します。
//    // */
//    //public void DeleteEndStation()
//    //{
//    //    if(_route.Paths.Count == 1)
//    //    {
//    //        DeleteStation(_route.Paths.First());
//    //        return;
//    //    }

//    //    UndoCommand deleteEndStationCommand = new UndoCommand();
//    //    Station deleteStation = _route.Paths.OrderBy(p => p.seq).Last().endStation;
//    //    deleteEndStationCommand.comment = $"DeleteEndPath";
//    //    deleteEndStationCommand.Invoke = () =>
//    //    {
//    //        _route.DeleteEndStation();
//    //        DiaFile.staticDia.SaveChanges();
//    //    };

//    //    deleteEndStationCommand.Undo = () =>
//    //    {
//    //        _route.AddStationEnd(deleteStation);
//    //        DiaFile.staticDia.SaveChanges();
//    //    };

//    //    deleteEndStationCommand.Redo = () =>
//    //    {
//    //        _route.DeleteEndStation();
//    //        DiaFile.staticDia.SaveChanges();
//    //    };
//    //    UndoStack.Instance.Push(deleteEndStationCommand);

//    //}
//}

public class VMRouteStation
{
    public string Name { get;  }
}

public class ModalPage : ContentPage
{
    public ModalPage(RouteEditPage page)
    {
        var main = new StationSelectorView();
        this.Content = main;
        main.OnStationSelected += Main_OnStationSelected;
    }

    private async void Main_OnStationSelected(object? sender, EventArgs e)
    {
        if(e is OnStationSelectedEventArgs args)
        {


            await Navigation.PopModalAsync();

        }
    }

    private async void CloseButton_Clicked(object sender, EventArgs e)
    {
    }
}