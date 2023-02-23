
using AOdiaData;
using CommunityToolkit.Maui.Views;

namespace AOdia5;


public partial class EditStationModal : Popup
{
    private INavigation Navigation;
    public EditStationModal(EditStationViewModel VM,INavigation navigation)
    {
        this.VM = VM;
        this.Navigation = navigation;
        InitializeComponent();
    }
 
    public EditStationViewModel VM{ 
        get { return BindingContext as EditStationViewModel; } 
        set { BindingContext = value; } }
    private void ChooseFromMap(object sender, EventArgs e)
    {
        MapViewModel viewModel= new MapViewModel();
        viewModel.editStation = VM;
        viewModel.stations = VM.stationListViewModel.Stations;
        Close();
        Navigation.PushAsync(new MapPage(viewModel));

    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        //ï¬Ç∂ÇÈ
    }

    private void DeleteStation(object sender, TappedEventArgs e)
    {
        //Ç±ÇÃâwÇçÌèúÇ∑ÇÈ
        VM.stationListViewModel.RemoveStation(VM.editStation);
        Close();


    }


    private void ClearName(object sender, TappedEventArgs e)
    {
        VM.editStation.Name.Value = "";

    }

    private void ReflectChange(object sender, EventArgs e)
    {
    }

    private void Cancel(object sender, EventArgs e)
    {
    }

    public void OpenView()
    {
 
    }
    public void CloseView()
    {

    }

}