
using System.Diagnostics;

namespace AOdia5;

public partial class MenuPage : ContentPage
{
	public MenuPage()
	{
		InitializeComponent();
	}

    private void Save(object sender, EventArgs e)
    {
        AOdiaData.DiaFile.staticDia.SaveChanges();
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if((sender as HorizontalStackLayout).BackgroundColor == Colors.LightGreen)
        {
            (FindByName("RoutesButton") as HorizontalStackLayout).BackgroundColor = Colors.Transparent;

        }
        else
        {
            (FindByName("RoutesButton") as HorizontalStackLayout).BackgroundColor = Colors.LightGreen;

        }

    }
}