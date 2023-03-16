
using AOdiaData;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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
}

public class RouteListPageModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
      => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


	private  readonly ObservableCollection<Route> _routes;
    public ObservableCollection<Route> routes { get { return _routes; } }

   public RouteListPageModel()
    {
        _routes = new ObservableCollection<Route>(DiaFile.staticDia.routes.Include(r=>r.Paths));
            _routes.CollectionChanged += OnPropertyChanged;
    }
    private void OnPropertyChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if(PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(routes)));

        }
    }
    public Route CreateNewRoute()
    {
       var res=Route.CreateNewRoute();
        DiaFile.staticDia.SaveChanges();
        return res;
        
    }


}