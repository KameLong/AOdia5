namespace AOdia5;

using AOdiaData;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Input;




public interface Bindable : INotifyPropertyChanged
{
    public void OnPropertyChanged([CallerMemberName] string propertyName = "");

}
public partial class MainPage : Shell
{
    public static INavigation navigation;

    public MainPage()
	{

        if (Device.Idiom == TargetIdiom.Phone)
        {
            FlyoutBehavior = FlyoutBehavior.Flyout;
        }
        else
        {
            FlyoutBehavior = FlyoutBehavior.Locked;
        }


        BindingContext = new MainPageModel();
        DiaFile.staticDia.OnSavedAction = () =>
        {
            if (Current.CurrentPage.BindingContext is Bindable notify)
            {
                notify.OnPropertyChanged("");
            }
        };
        Routing.RegisterRoute("Station", typeof(StationListPage));
        Routing.RegisterRoute("Route", typeof(RouteListPage));
        Routing.RegisterRoute("Route/edit", typeof(RouteEditPage));
        Routing.RegisterRoute("test/keyTest", typeof(KeyEventTest));
        InitializeComponent();
    }



    private async void MenuItem_Clicked(object sender, EventArgs e)
    {


    }

    private void Undo(object sender, EventArgs e)
    {
        UndoStack.Instance.Undo();
    }

    private void Redo(object sender, EventArgs e)
    {
        UndoStack.Instance.Redo();

    }

    private void Back(object sender, EventArgs e)
    {
        string oldUrl=UndoStack.Instance.PopUrl();
        string url = UndoStack.Instance.PopUrl(); ;
        
        Shell.Current.Goto(url,oldUrl);
    }
}
class MainPageModel:INotifyPropertyChanged
{
    public ICommand MenuItem_Clicked { get; }
    public MainPageModel()
    {
        MenuItem_Clicked = new Command(
        execute: (object URL) =>
        {
            Shell.Current.Goto(URL.ToString() );
        },
        canExecute: (object URL) =>
        {
            return true;
        });

        UndoStack.Instance.PropertyChanged += (object sender, PropertyChangedEventArgs e) =>
        {
            OnPropertyChanged(nameof(UndoEble));
            OnPropertyChanged(nameof(RedoEble));
            OnPropertyChanged(nameof(BackEble));
        };
    }
    public bool BackEble { get { return UndoStack.Instance.CanBack(); } }

    public bool UndoEble { get { return UndoStack.Instance.CanUndo(); } }
    public bool RedoEble { get { return UndoStack.Instance.CanRedo(); } }

    public event PropertyChangedEventHandler? PropertyChanged;
    public virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
      => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

}



