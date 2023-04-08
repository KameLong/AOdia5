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
    private StationSelectorViewModel VM { get { return (StationSelectorViewModel)BindingContext; } }
    public StationListPage()
	{
        
        InitializeComponent();
        BindingContext = new StationSelectorViewModel();
    }

    private void SelectedStation(object sender, SelectedItemChangedEventArgs e)
    {
        // リストビューで選択されたアイテムを取得する。
        ListView listView = (ListView)sender;
        Station station = (Station)listView.SelectedItem;
        EditStationViewModel vm = new EditStationViewModel { editStation = station, stationListViewModel = VM };
        this.ShowPopup(new EditStationModal(vm,Navigation));
    }
    private void OnClickAddStation(object sender, TappedEventArgs e)
    {
        Station station=VM.AddNewStation();
        EditStationViewModel vm= new EditStationViewModel { editStation = station, stationListViewModel = VM };
        this.ShowPopup(new EditStationModal(vm,Navigation));
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
}

