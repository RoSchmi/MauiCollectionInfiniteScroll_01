using MauiCollectionInfiniteScroll_01.ViewModels;


namespace MauiCollectionInfiniteScroll_01.Pages;

public partial class DisplayPage : ContentPage
{
	public DisplayPage(DisplayViewModel vm)
	{
		InitializeComponent();   
        BindingContext = vm;
    }
}