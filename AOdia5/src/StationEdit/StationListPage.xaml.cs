using AOdiaData;
using CommunityToolkit.Maui.Views;
using Reactive.Bindings;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace AOdia5;


public class StationSearchHandler : SearchHandler
{
    public static readonly BindableProperty StationsProperty = BindableProperty.Create(
      nameof(Stations), // プロパティ名
      typeof(List<Station>), // プロパティの型
      typeof(StationSearchHandler), // プロパティを持つ View の型
      new List<Station>(), // 初期値
      BindingMode.TwoWay, // バインド方向
      propertyChanged: (bindable, oldValue, newValue) => { // 変更通知ハンドラ
          Debug.WriteLine("OK");
      }
     ); // BindableProperty.CoerceValueDelegate Xamarin 公式にも説明なしなので用途不明


    public ObservableCollection<Station> Stations { get { return (ObservableCollection<Station>)this.GetValue(StationsProperty); } set{} }

    protected override void OnQueryChanged(string oldValue, string newValue)
    {
        base.OnQueryChanged(oldValue, newValue);

        if (string.IsNullOrWhiteSpace(newValue))
        {
            ItemsSource = null;
        }
        else
        {
            ItemsSource = Stations
                .Where(station => station.Name.Value.ToLower().Contains(newValue.ToLower()))
                .ToList<Station>();
        }
    }

    protected override  void OnItemSelected(object item)
    {
        base.OnItemSelected(item);

    }

    //string GetNavigationTarget()
    //{
    //    return (Shell.Current as AppShell).Route.FirstOrDefault(route => route.Value.Equals(SelectedItemNavigationTarget)).Key;
    //}
}

public partial class StationListPage : ContentPage
{
    private StationListViewModel VM { get { return (StationListViewModel)BindingContext; } }
    public StationListPage()
	{
        
        InitializeComponent();
        BindingContext = new StationListViewModel();
    }

    private void SelectedStation(object sender, SelectedItemChangedEventArgs e)
    {
        // リストビューで選択されたアイテムを取得する。
        ListView listView = (ListView)sender;
        Station station = (Station)listView.SelectedItem;
        EditStationViewModel vm = new EditStationViewModel { editStation = station, stationListViewModel = VM };
        this.ShowPopup(new EditStationModal(vm,Navigation));
    }
    private void OnClickAddStation(object sender, TappedEventArgs e)
    {
        Station station=VM.AddNewStation();
        EditStationViewModel vm= new EditStationViewModel { editStation = station, stationListViewModel = VM };
        this.ShowPopup(new EditStationModal(vm,Navigation));
    }

}
