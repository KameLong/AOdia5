using AOdiaData;
using CommunityToolkit.Maui.Core.Extensions;
using Reactive.Bindings;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Path = AOdiaData.Path;

namespace AOdia5;

public partial class RouteEditPage : ContentPage
{

    private RouteEditPageModel VM { get { return BindingContext as RouteEditPageModel; } set { BindingContext = value; } }
    public RouteEditPage(RouteEditPageModel vm) : this()
    {
        VM = vm;
    }

    public RouteEditPage()
	{
		InitializeComponent();
	}
}


public class RouteEditPageModel : INotifyPropertyChanged
{

    public event PropertyChangedEventHandler PropertyChanged;
    public virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
      => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


    private readonly Route _route;

    public ReactiveProperty<Route> route { get { return new ReactiveProperty<Route>(_route); } }

    private readonly ObservableCollection<Path> _paths;
    public ObservableCollection<Path> paths {get{return _paths;}}

    private RouteListPageModel routeListPageModel { get; set; }

	public RouteEditPageModel(Route route, RouteListPageModel routeListPageModel)
    {
        _route = route;
        this.routeListPageModel = routeListPageModel;
        _paths = route.Paths.ToObservableCollection();
        _paths.CollectionChanged += OnPropertyChanged;
    }
    public RouteEditPageModel()
    {
        _route = new Route();
        _paths = new ObservableCollection<Path>();
        _paths.CollectionChanged += OnPropertyChanged;
    }



    private void OnPropertyChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        PropertyChanged(this, new PropertyChangedEventArgs("paths"));
    }



}

