using AOdiaData;
using CommunityToolkit.Maui.Core.Extensions;
using Reactive.Bindings;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AOdia5;

public partial class RouteEditFromMapPage : ContentPage
{
	public RouteEditFromMapPage()
	{
		InitializeComponent();
	}
}

public class RouteEditFromMapPageModel : INotifyPropertyChanged
{

    public event PropertyChangedEventHandler PropertyChanged;
    public virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
      => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));



    private readonly ObservableCollection<Route> _routes;
    public ObservableCollection<Route> routes { get { return _routes; } }

    private readonly ObservableCollection<Station> _stations;
    public ObservableCollection<Station> stations { get { return _stations; } }



    public RouteEditFromMapPageModel(ICollection<Route> routes, ICollection<Station>stations)
    {
        _routes = routes.ToObservableCollection();
        _stations = stations.ToObservableCollection();

        _routes.CollectionChanged += 
            (object sender, NotifyCollectionChangedEventArgs e)=> { PropertyChanged(this, new PropertyChangedEventArgs("routes")); };
        _stations.CollectionChanged +=
            (object sender, NotifyCollectionChangedEventArgs e) => { PropertyChanged(this, new PropertyChangedEventArgs("stations")); };
    }



}

