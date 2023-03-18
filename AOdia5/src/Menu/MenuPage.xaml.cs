
using AOdiaData;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace AOdia5;

public partial class MenuPage : ContentPage, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
      => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    
    public DataModel model=new DataModel();



    public string test { get { return model.Text; } 
        set {
            var comm = createUpdateCommand(model, value);
            UndoStack.Instance.Push(comm);
            Debug.WriteLine(value);
        } }
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
        UndoStack.Instance.Undo();
        //        AOdiaData.DiaFile.staticDia.SaveChanges();

    }

    public AOdiaData.UndoCommand createUpdateCommand(DataModel model, string newText)
    {
        string prevText = null;
        UndoCommand cmd = new AOdiaData.UndoCommand();
        cmd.Invoke = () =>
        {
            prevText = model.Text;
            model.Text = newText;
            OnPropertyChanged(nameof(test));
        };

        cmd.Undo = () =>
        {
            model.Text = prevText;
            OnPropertyChanged(nameof(test));
        };

        cmd.Redo = () =>
        {
            model.Text = newText;
            OnPropertyChanged(nameof(test));
        };

        return cmd;
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

public class DataModel: INotifyPropertyChanged
{
    public string Text = "";
    public virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
  => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public event PropertyChangedEventHandler? PropertyChanged;
}