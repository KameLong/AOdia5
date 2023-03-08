
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
        //        Navigation.PushAsync(new RouteEditFromMapPage());
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
        _routes = new ObservableCollection<Route>(DiaFile.staticDia.routes.Include(r=>r.Paths));
        _routes.CollectionChanged += OnPropertyChanged;
    }
    private void OnPropertyChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        PropertyChanged(this, new PropertyChangedEventArgs("routes"));
    }


}