
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
	private RouteListPageModel VM { get { return BindingContext as RouteListPageModel; }set { BindingContext = value; } }
	public RouteListPage(RouteListPageModel vm):this()
	{
		VM = vm;
	}
	public RouteListPage()
	{
		VM= new RouteListPageModel();
		InitializeComponent();






    }

    /*
     * ListView‚Å‘I‘ð‚µ‚½˜Hü‚Ì•ÒW‚É‘JˆÚ‚·‚é
     */
    private void EditRoute(object sender, TappedEventArgs e)
    {
        if (e.Parameter is VMRoute route)
        {
            Shell.Current.Goto($"Route/edit?routeID={route.routeID}");
        }
    }

    /*
     * V˜Hü‚ð’Ç‰Á‚·‚é
     * RouteEdit‚ÉˆÚ“®‚·‚é
     */
    private async void AddNewRoute(object sender, EventArgs e)
    {
        string? routeName = await DisplayPromptAsync("Route name", "Write name of new route.");
        if (routeName == null)
        {
            return;
        }
        Route route=VM.AddNewRoute(routeName);
        Shell.Current.Goto($"Route/edit?routeID={route.RouteId}");
    }

    private void DeleteRoute(object sender, TappedEventArgs e)
    {
        if(e.Parameter is VMRoute route){
            VM.DeleteRoute(route);
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
                    Shell.Current.Goto($"Route/edit?routeID={route.routeID}");
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
                    VM.DeleteRoute(route);
                    routeList.SelectedItem = null;
                }
            }
        }
        if(keyCode == AOdiaKey.Enter) { }

        if (modifierKey.AltPressed && keyCode == AOdiaKey.Left)
        {
            Debug.WriteLine("–ß‚é");
        }
    }

    private void routeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Debug.WriteLine("‚¹‚ê‚­‚Ä‚Á‚Ç");
    }

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        VM.SearchText = e.NewTextValue;
        VM.OnPropertyChanged(nameof(VM.searchedRoutes));
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
public class RouteListPageModel : Bindable
{

    public event PropertyChangedEventHandler? PropertyChanged;
    public  void OnPropertyChanged([CallerMemberName] string propertyName = "")
      => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public List<VMRoute> routes;
    public string SearchText { get; set; } = "";
    public List<VMRoute> searchedRoutes { get {
            return routes.Where(r => r.name.Contains(SearchText)).ToList();
        } }




    public RouteListPageModel()
    {
        routes=DiaFile.staticDia.routes.OrderBy(r => r.Name).Select(r => new VMRoute(r)).ToList();
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
            OnPropertyChanged(nameof(routes));
        };

        deleteRouteCmd.Undo = () =>
        {
            DiaFile.staticDia.routes.Add(route.route);
            DiaFile.staticDia.SaveChanges();
            OnPropertyChanged(nameof(routes));
        };

        deleteRouteCmd.Redo = () =>
        {
            DiaFile.staticDia.routes.Remove(route.route);
            DiaFile.staticDia.SaveChanges();
            OnPropertyChanged(nameof(routes));
        };
        UndoStack.Instance.Push(deleteRouteCmd);

    }


}