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
      nameof(Stations), // �v���p�e�B��
      typeof(List<Station>), // �v���p�e�B�̌^
      typeof(StationSearchHandler), // �v���p�e�B������ View �̌^
      new List<Station>(), // �����l
      BindingMode.TwoWay, // �o�C���h����
      propertyChanged: (bindable, oldValue, newValue) => { // �ύX�ʒm�n���h��
          Debug.WriteLine("OK");
      }
     ); // BindableProperty.CoerceValueDelegate Xamarin �����ɂ������Ȃ��Ȃ̂ŗp�r�s��


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
        // ���X�g�r���[�őI�����ꂽ�A�C�e�����擾����B
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
