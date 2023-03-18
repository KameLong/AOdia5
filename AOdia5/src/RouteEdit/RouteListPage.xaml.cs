
using AOdiaData;
using Microsoft.EntityFrameworkCore;
using Reactive.Bindings.Extensions;
using Reactive.Bindings;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;
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
    private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem != null)
        {
            ListView listView = (ListView)sender;
            Route route = (Route)listView.SelectedItem;

            RouteEditPageModel vm = new RouteEditPageModel(route, VM);
            Navigation.PushAsync(new RouteEditPage(vm));
            listView.SelectedItem = null;

        }
    }

    /*
     * V˜Hü‚ð’Ç‰Á‚·‚é
     * RouteEdit‚ÉˆÚ“®‚·‚é
     */
    private void AddNewRoute(object sender, EventArgs e)
    {
        Route route = VM.CreateNewRoute();
        RouteEditPageModel vm = new RouteEditPageModel(route, VM);
        Navigation.PushAsync(new RouteEditPage(vm));
    }

    private void DeleteRoute(object sender, TappedEventArgs e)
    {
        if(e.Parameter is Route route){
            VM.DeleteRoute(route);
        }
    }
}

public class RouteListPageModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
      => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));




    public ObservableCollection<Route> routes { get { return DiaFile.staticDia.routes.Include(r => r.Paths).ToObservableCollection(); } }






    public RouteListPageModel()
    {
    }
    public Route CreateNewRoute()
    {
       var res=Route.CreateNewRoute();
        DiaFile.staticDia.SaveChanges();
        return res;
        
    }
    public void DeleteRoute(Route route)
    {
        UndoCommand deleteRouteCmd = new UndoCommand();
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