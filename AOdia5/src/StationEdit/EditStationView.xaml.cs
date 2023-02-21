
using AOdiaData;

namespace AOdia5;

internal interface OpenCloseEditStationView
{
    void CloseEditStationView(EditStationView editStationView);
    void OpenEditStationView(EditStationView editStationView);
}
public partial class EditStationView : ContentView
{
    internal EditStationViewModel VM { get{ return (EditStationViewModel)BindingContext;}}
	public EditStationView()
	{
        BindingContext = new EditStationViewModel();
        InitializeComponent();
	}

    private void ChooseFromMap(object sender, EventArgs e)
    {
        MapViewModel viewModel= new MapViewModel();
        viewModel.editStation = VM;
        viewModel.stations = VM.stationListViewModel.Stations;
        Application.Current.MainPage = new MapPage(viewModel);

    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        //ï¬Ç∂ÇÈ
    }

    private void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        //Ç±ÇÃâwÇçÌèúÇ∑ÇÈ
        VM.stationListViewModel.RemoveStation(VM.editStation);

        if(this.Parent.Parent.Parent is OpenCloseEditStationView)
        {
            (this.Parent.Parent.Parent as OpenCloseEditStationView).CloseEditStationView(this);
        }

    }
}