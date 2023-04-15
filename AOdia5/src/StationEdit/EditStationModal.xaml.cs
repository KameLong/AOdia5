
using AOdiaData;
using CommunityToolkit.Maui.Views;

namespace AOdia5;


public partial class EditStationView : VerticalStackLayout
{
    public EditStationView()
    {
        this.BindingContext = this;
        InitializeComponent();
    }
 
    private void ChooseFromMap(object sender, EventArgs e)
    {
        MapViewModel viewModel = new MapViewModel
        {
        };
        Navigation.PushAsync(new MapPage(viewModel));

    }

    private void DeleteStation(object sender, TappedEventArgs e)
    {
        //Ç±ÇÃâwÇçÌèúÇ∑ÇÈ
    }

    private void ClearName(object sender, TappedEventArgs e)
    {
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