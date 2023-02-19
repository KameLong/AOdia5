using AOdiaData;
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


    public List<Station> Stations { get { return (List<Station>)this.GetValue(StationsProperty); } set{} }
    public Type SelectedItemNavigationTarget { get; set; }

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
                .Where(station => station.Name.ToLower().Contains(newValue.ToLower()))
                .ToList<Station>();
        }
    }

    protected override async void OnItemSelected(object item)
    {
        base.OnItemSelected(item);

        // Let the animation complete
        await Task.Delay(1000);

        ShellNavigationState state = (App.Current.MainPage as Shell).CurrentState;
        // The following route works because route names are unique in this app.
//        await Shell.Current.GoToAsync($"{GetNavigationTarget()}?name={((Station)item).Name}");
    }

    //string GetNavigationTarget()
    //{
    //    return (Shell.Current as AppShell).Route.FirstOrDefault(route => route.Value.Equals(SelectedItemNavigationTarget)).Key;
    //}
}

public partial class StationList : ContentPage
{

    public StationList()
	{
        InitializeComponent();
    }
    public void OnClickAddStation(object sender, EventArgs args)
    {
        //駅を追加します。
    }

    private void OnStationEditIconClicked(object sender, TappedEventArgs e)
    {
        //駅編集がクリックされました
    }

    private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
                ListView listView = (ListView)sender;
        // リストビューで選択されたアイテムを取得する。
        Station station = (Station)listView.SelectedItem;

        //駅編集がクリックされました
        editStationView.BindingContext = new EditStationViewModel {editStation=station,stationListViewModel=(StationListViewModel)BindingContext };
        editStationFrame.ZIndex = 10;

    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        editStationFrame.ZIndex = -10;

    }
}
