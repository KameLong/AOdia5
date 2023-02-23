using AOdiaData;

namespace AOdia5;

public partial class MenuPage : ContentPage
{
	public MenuPage()
	{
		InitializeComponent();
	}

    private void Save(object sender, EventArgs e)
    {
		StaticData.staticDia.SaveChanges();
    }
}