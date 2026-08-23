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
}