using System.Diagnostics;
using MauiCollectionInfiniteScroll_01.Models;
using MauiCollectionInfiniteScroll_01.ViewModels;

#if ANDROID
using Android.Test.Suitebuilder.Annotation;
using AndroidX.RecyclerView.Widget;
using static AndroidX.RecyclerView.Widget.RecyclerView;
#endif


namespace MauiCollectionInfiniteScroll_01.Pages;

public partial class DisplayPage : ContentPage
{
    bool _syncing = false;

#if ANDROID
    private RecyclerView? _recyclerView;
#endif
    public DisplayPage(DisplayViewModel vm)
	{
		InitializeComponent();   
        BindingContext = vm;

        Loaded += OnLoaded;
    }

#if ANDROID
    private async void RecyclerView_LayoutChange(object? sender, Android.Views.View.LayoutChangeEventArgs e)
    {
        Debug.WriteLine($"\r\nRecyclerView_LayoutChanged at {DateTime.Now:HHH:mm:ss.fff}\r\n");
    }
#endif

    private void OnLoaded(object sender, EventArgs e)
    {
        // Hier ist die Page vollständig aufgebaut
        if (BindingContext is DisplayViewModel vm)
        {
            vm.IsBusy = true;
            vm.IsBusy = false;
      
#if ANDROID

        var type = DisplayCollectionView.Handler?.PlatformView?.GetType();

        _recyclerView = DisplayCollectionView.Handler?.PlatformView as RecyclerView;

        if (_recyclerView != null)
        {
            _recyclerView.HasFixedSize = true;       // important. Prevents stuttering vertical scrolling

            _recyclerView.LayoutChange += RecyclerView_LayoutChange;

            _recyclerView.ViewTreeObserver.GlobalLayout += (_, _) =>
            {
                Debug.WriteLine($"GlobalLayout {DateTime.Now:HH:mm:ss.fff}");
            };
            vm.RequestSuppressLayout = suppress =>
            {
                _recyclerView?.SuppressLayout(suppress);

            };
        }

        Debug.WriteLine($"\r\n Printing androidhandler \r\n");

        Debug.WriteLine($"Type:      {type?.FullName}");
        Debug.WriteLine($"BaseType:  {type?.BaseType?.FullName}");
        Debug.WriteLine($"BaseType2: {type?.BaseType?.BaseType?.FullName}");
        Debug.WriteLine($"Layout Manager is:  {_recyclerView?.GetLayoutManager()?.GetType().FullName}");
        Debug.WriteLine($"\r\n Printed androidhandler \r\n");

#endif
        }

    }

    private void OnVerticallyScrolledCollection(object sender, ItemsViewScrolledEventArgs e)
    {
        if (BindingContext is DisplayViewModel vm)
        {
            vm.ActualTopVisibleItemIndex = e.FirstVisibleItemIndex;
            vm.ActualBottomVisibleItemIndex = e.LastVisibleItemIndex;

            Debug.WriteLine($"VerticallyScrolled {DateTime.Now:HH:mm:ss.fff}  FirstVisibleRow: {e.FirstVisibleItemIndex}");
            vm.VerticallyScrolledCollection(e);
        }
    }

    private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var item = e.CurrentSelection.FirstOrDefault() as EntityDisplayItem;
        if (item == null)
            return;

        // ViewModel holen
        var vm = BindingContext as DisplayViewModel;

        // Command ausführen
        vm?.ItemSelectedCommand.Execute(item); 
    }
    
    private void OnScrolledHeader(object sender, ScrolledEventArgs e)
    {
        if (_syncing)
            return;

        _syncing = true;
        ScrollTable.ScrollToAsync(e.ScrollX, 0, false);
        _syncing = false;
    }

    private void OnScrolledTable(object sender, ScrolledEventArgs e)
    {
        if (_syncing)
            return;

        _syncing = true;
        ScrollHeader.ScrollToAsync(e.ScrollX, 0, false);
        _syncing = false;
    }
}