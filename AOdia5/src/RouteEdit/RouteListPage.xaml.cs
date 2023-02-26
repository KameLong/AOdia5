using AOdiaData;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Reactive.Bindings;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

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
//		VM.routes = StaticData.staticDia.routes.ToObservableCollection();
		InitializeComponent();
        
	}

    private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        // リストビューで選択されたアイテムを取得する。
        ListView listView = (ListView)sender;
        Route route = (Route)listView.SelectedItem;


        RouteEditPageModel vm = new RouteEditPageModel(route,VM);
        
        Navigation.PushAsync(new RouteEditPage(vm));


    }

    private void Button_Clicked(object sender, EventArgs e)
    {
    }
}

public class RouteListPageModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    public virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
      => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


	private  readonly ObservableCollection<Route> _routes;
    public ObservableCollection<Route> routes { get { return _routes; } }



   public RouteListPageModel()
    {
        _routes = new ObservableCollection<Route>(AOdiaData.AOdiaData.staticDia.routes.Include(r => r.Paths));
        _routes.CollectionChanged += OnPropertyChanged;
    }
    private void OnPropertyChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        PropertyChanged(this, new PropertyChangedEventArgs("routes"));
    }


}