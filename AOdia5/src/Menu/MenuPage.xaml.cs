
using AOdiaData;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace AOdia5;

public partial class MenuPage : ContentPage
{

    



	public MenuPage()
	{
        BindingContext = this;
		InitializeComponent();
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

    private void Save(object sender, TappedEventArgs e)
    {
        AOdiaData.DiaFile.staticDia.SaveChanges();

    }


    private void Undo(object sender, EventArgs e)
    {
        UndoStack.Instance.Undo();
    }
    private void Redo(object sender, EventArgs e)
    {
        UndoStack.Instance.Redo();
    }
}

