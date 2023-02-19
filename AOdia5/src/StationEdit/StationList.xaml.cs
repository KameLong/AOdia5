using AOdiaData;
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
        //�w��ǉ����܂��B
    }

    private void OnStationEditIconClicked(object sender, TappedEventArgs e)
    {
        //�w�ҏW���N���b�N����܂���
    }

    private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
                ListView listView = (ListView)sender;
        // ���X�g�r���[�őI�����ꂽ�A�C�e�����擾����B
        Station station = (Station)listView.SelectedItem;

        //�w�ҏW���N���b�N����܂���
        editStationView.BindingContext = new EditStationViewModel {editStation=station,stationListViewModel=(StationListViewModel)BindingContext };
        editStationFrame.ZIndex = 10;

    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        editStationFrame.ZIndex = -10;

    }
}
