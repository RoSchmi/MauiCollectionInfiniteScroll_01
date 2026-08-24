using MauiCollectionInfiniteScroll_01.Models;
using MauiCollectionInfiniteScroll_01.ViewModels;


namespace MauiCollectionInfiniteScroll_01.Pages;

public partial class DisplayPage : ContentPage
{
	public DisplayPage(DisplayViewModel vm)
	{
		InitializeComponent();   
        BindingContext = vm;

        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, EventArgs e)
    {
        // Hier ist die Page vollständig aufgebaut
        if (BindingContext is DisplayViewModel vm)
        {
            vm.IsBusy = true;
            vm.IsBusy = false;
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
}