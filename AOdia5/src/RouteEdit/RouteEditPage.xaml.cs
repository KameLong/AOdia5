using AOdiaData;
using CommunityToolkit.Maui.Core.Extensions;
using Reactive.Bindings;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Windows.Input;
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

    private void Button_Clicked(object sender, EventArgs e)
    {
        Debug.WriteLine("test");
    }
}


public class RouteEditPageModel : INotifyPropertyChanged
{

    public event PropertyChangedEventHandler PropertyChanged;
    public virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
      => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


    private readonly Route _route;

    public ReactiveProperty<Route> route { get { return new ReactiveProperty<Route>(_route); } }

    private  ObservableCollection<Path> _paths;

    private readonly ObservableCollection<Station> _stations;
    public  ObservableCollection<Station> stations { get { return _stations; } }

    private RouteListPageModel? routeListPageModel { get; set; }

    public ReactiveProperty<string> routeColorHtml {
        get {
            string s = String.Format("{0:X4}", _route.color.Value.ToArgb());

            return new ReactiveProperty<string>($"#{s.Substring(2)}{s.Substring(0,2)}");
        }set {
            if (value.Value.Length > 1) {
                var color="";
                switch(value.Value.Length)
                {
                    case 4:
                        color = $"#{value.Value[1]}{value.Value[1]}{value.Value[2]}{value.Value[2]}{value.Value[3]}{value.Value[3]}FF";
                        break;
                    case 5:
                        color = $"#{value.Value[1]}{value.Value[1]}{value.Value[2]}{value.Value[2]}{value.Value[3]}{value.Value[3]}{value.Value[4]}{value.Value[4]}";
                        break;
                    case 7:
                        color = value.Value + "FF";
                        break;
                    case 9:
                        color = value.Value;
                        break;
                    default:
                        color ="#000000FF";
                        break;
                }

                _route.color.Value = System.Drawing.Color.FromArgb(Convert.ToInt32(color.Substring(7, 2), 16), Convert.ToInt32(color.Substring(1, 2), 16), Convert.ToInt32(color.Substring(3, 2), 16), Convert.ToInt32(color.Substring(5, 2), 16));
            }
        
        } }


	public RouteEditPageModel(Route route, RouteListPageModel routeListPageModel)
    {
        _route = route;
        this.routeListPageModel = routeListPageModel;
        _paths = route.Paths.ToObservableCollection();
        _stations=_paths.OrderBy(p=>p.seq).Select(p=>p.startStation).ToObservableCollection();
        _stations.Add(_paths.OrderBy(p => -p.seq).First().endStation);

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

