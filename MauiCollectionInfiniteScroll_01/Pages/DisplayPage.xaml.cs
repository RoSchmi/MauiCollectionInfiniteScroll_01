using MauiCollectionInfiniteScroll_01.ViewModels;


namespace MauiCollectionInfiniteScroll_01.Pages;

public partial class DisplayPage : ContentPage
{
	public DisplayPage(DisplayViewModel vm)
	{
		InitializeComponent();   
        BindingContext = vm;
        // Needed for correct positioning of ActivityIndicator
        Loaded += (_, _) =>
        {
            vm.IsBusy = false;
        };
    }
}