namespace AOdia5;

public partial class SimpleStationView : ContentView
{
    public SimpleStationView()
    {
        InitializeComponent();
    }
    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        if (width < 0)
        {
            return;
        }
        if (width < 500)
        {
            forPC.IsVisible = false;
            forPhone.IsVisible = true;
        }
        else
        {
            forPhone.IsVisible = false;
            forPC.IsVisible = true;

        }
    }
}