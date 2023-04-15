using AOdiaData;
using CommunityToolkit.Maui.Views;
using Reactive.Bindings;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;

namespace AOdia5;




public partial class StationListPage : ContentPage
{
    public StationListPage()
	{
        InitializeComponent();
    }

    private void OnClickAddStation(object sender, TappedEventArgs e)
    {
        //Station station=VM.AddNewStation();
        //EditStationViewModel vm= new EditStationViewModel { editStation = station, stationListViewModel = VM };
        //this.ShowPopup(new EditStationModal(vm,Navigation));
    }

    private void OnStationClicked(object sender, TappedEventArgs e)
    {
        if(sender is HorizontalStackLayout layout&&layout.BindingContext is VMStation station)
        {
            Debug.WriteLine(station.name);
        }
    }

    private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is HorizontalStackLayout layout && layout.BindingContext is VMStation station)
        {
            Debug.WriteLine(station.name);
        }
    }

    private void OnStationSelected(object sender, EventArgs e)
    {
        if(e is OnStationSelectedEventArgs args)
        {
            Debug.WriteLine(args.station.Name);
//            Shell.Current.Goto($"station/edit?stationID={args.station.StationId}");
        }

    }
}

