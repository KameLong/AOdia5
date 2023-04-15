
using AOdiaData;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Maui.Core.Extensions;
using System.Diagnostics;

namespace AOdia5;

public partial class RouteListPage : ContentPage,KeyEventListener
{
	public RouteListPage()
	{
        this.BindingContext = this;
        InitializeComponent();
    }


    /*
     * ListViewÇ≈ëIëÇµÇΩòHê¸ÇÃï“èWÇ…ëJà⁄Ç∑ÇÈ
     */
    private void EditRoute(object sender, TappedEventArgs e)
    {
        if (e.Parameter is VMRoute route)
        {
            Shell.Current.Goto($"//routeEdit?routeID={route.routeID}");
        }
    }
    /*
     * êVòHê¸Çí«â¡Ç∑ÇÈ
     * RouteEditÇ…à⁄ìÆÇ∑ÇÈ
     */
    private async void AddNewRoute(object sender, EventArgs e)
    {
        string? routeName = await DisplayPromptAsync("Route name", "Write name of new route.");
        if (routeName == null)
        {
            return;
        }
        Route route = AddNewRoute(routeName);
        Shell.Current.Goto($"//routeEdit?routeID={route.RouteId}");
    }
    private void DeleteRoute(object sender, TappedEventArgs e)
    {
        if(e.Parameter is VMRoute route){
            DeleteRoute(route);
        }
    }
     
    public void OnKeyPress(AOdiaKey keyCode, ModifierKey modifierKey)
    {
        if(keyCode == AOdiaKey.Enter)
        {
            Debug.WriteLine("Enter");
            if (routeList.SelectedItem != null)
            {
                if (routeList.SelectedItem is VMRoute route)
                {
                    Shell.Current.Goto($"//routeEdit?routeID={route.routeID}");
                }
            }
        }
        if (keyCode == AOdiaKey.Delete)
        {
            Debug.WriteLine("Delete");
            if (routeList.SelectedItem != null)
            {
                if (routeList.SelectedItem is VMRoute route)
                {
                    Debug.WriteLine($"Delete:{route.name}");
                    DeleteRoute(route);
                    routeList.SelectedItem = null;
                }
            }
        }
        if (modifierKey.AltPressed && keyCode == AOdiaKey.Left)
        {
            Debug.WriteLine("ñﬂÇÈ");
        }
    }

    private void routeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Debug.WriteLine("ÇπÇÍÇ≠ÇƒÇ¡Ç«");
    }

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        SearchText = e.NewTextValue;
        OnPropertyChanged(nameof(searchedRoutes));
    }


    public string SearchText { get; set; } = "";
    public IEnumerable<VMRoute> searchedRoutes
    {
        get
        {
            return DiaFile.staticDia.routes.Where(r => r.Name.Contains(SearchText)).OrderBy(r => r.Name).Select(r => new VMRoute(r));
        }
    }

    public Route AddNewRoute(string routeName)
    {
        var route = Route.CreateNewRoute();
        route.Name = routeName;

        UndoCommand addNewRouteCmd = new UndoCommand();
        addNewRouteCmd.comment = $"AddNewRoute({route.RouteId})";
        addNewRouteCmd.Invoke = () =>
        {
            DiaFile.staticDia.SaveChanges();
        };

        addNewRouteCmd.Undo = () =>
        {
            DiaFile.staticDia.routes.Remove(route);
            DiaFile.staticDia.SaveChanges();
        };

        addNewRouteCmd.Redo = () =>
        {
            DiaFile.staticDia.routes.Add(route);
            DiaFile.staticDia.SaveChanges();
        };
        UndoStack.Instance.Push(addNewRouteCmd);
        return route;
    }
    public void DeleteRoute(VMRoute route)
    {
        UndoCommand deleteRouteCmd = new UndoCommand();
        deleteRouteCmd.comment = $"deleteroute({route.route.RouteId})";
        deleteRouteCmd.Invoke = () =>
        {
            DiaFile.staticDia.routes.Remove(route.route);
            DiaFile.staticDia.SaveChanges();
//            OnPropertyChanged(nameof(routes));
        };

        deleteRouteCmd.Undo = () =>
        {
            DiaFile.staticDia.routes.Add(route.route);
            DiaFile.staticDia.SaveChanges();
//            OnPropertyChanged(nameof(routes));
        };

        deleteRouteCmd.Redo = () =>
        {
            DiaFile.staticDia.routes.Remove(route.route);
            DiaFile.staticDia.SaveChanges();
//            OnPropertyChanged(nameof(routes));
        };
        UndoStack.Instance.Push(deleteRouteCmd);

    }

}


public class VMRoute
{
    public Route route;
    public VMRoute(Route route) {
        this.route = route;
    }
    public string name { get { return route.Name; } }
    public long routeID { get { return route.RouteId; } }
}
