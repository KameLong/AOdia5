
using AOdiaData;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Maui.Core.Extensions;


namespace AOdia5;

public partial class RouteListPage : ContentPage
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
        if (e.Parameter is Route route)
        {
            Shell.Current.Goto($"Route/edit?routeID={route.RouteId}");
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
        if(e.Parameter is Route route){
            VM.DeleteRoute(route);
        }
    }
}

public class RouteListPageModel : Bindable
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public  void OnPropertyChanged([CallerMemberName] string propertyName = "")
      => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public ObservableCollection<Route> routes { get { return DiaFile.staticDia.routes.Include(r => r.Paths).ToObservableCollection(); } }






    public RouteListPageModel()
    {
    }
    public Route AddNewRoute(string routeName)
    {
        var route = Route.CreateNewRoute();
        route.Name.Value = routeName;

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
    public void DeleteRoute(Route route)
    {
        UndoCommand deleteRouteCmd = new UndoCommand();
        deleteRouteCmd.comment = $"deleteroute({route.RouteId})";
        deleteRouteCmd.Invoke = () =>
        {
            DiaFile.staticDia.routes.Remove(route);
            DiaFile.staticDia.SaveChanges();
            OnPropertyChanged(nameof(routes));
        };

        deleteRouteCmd.Undo = () =>
        {
            DiaFile.staticDia.routes.Add(route);
            DiaFile.staticDia.SaveChanges();
            OnPropertyChanged(nameof(routes));
        };

        deleteRouteCmd.Redo = () =>
        {
            DiaFile.staticDia.routes.Remove(route);
            DiaFile.staticDia.SaveChanges();
            OnPropertyChanged(nameof(routes));
        };
        UndoStack.Instance.Push(deleteRouteCmd);

    }


}