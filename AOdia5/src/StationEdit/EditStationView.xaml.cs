
namespace AOdia5;

public partial class EditStationView : ContentView
{
	public EditStationView()
	{
        BindingContext = new EditStationViewModel();
        InitializeComponent();
	}

    private void ChooseFromMap(object sender, EventArgs e)
    {
        MapViewModel viewModel= new MapViewModel();
        viewModel.editStation = (EditStationViewModel)this.BindingContext;
        viewModel.stations = ((EditStationViewModel)this.BindingContext).stationListViewModel.Stations;
        Application.Current.MainPage = new MapPage(viewModel);

    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        //•Â‚¶‚é
    }
}