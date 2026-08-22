using MauiCollectionInfiniteScroll_01.ViewModels;

namespace MauiCollectionInfiniteScroll_01
{
    public partial class MainPage : ContentPage
    {
        // For MVVM in .xaml has to be included:            
        // xmlns:pagemodel="clr-namespace:MauiCollectionInfiniteScroll_01"
        // xmlns:viewmodels="clr-namespace:MauiCollectionInfiniteScroll_01.ViewModels"
        // x:DataType="viewmodels:MainPageViewModel">
        // In 'MauiProgram.cs' References to MainPage and MainPageViewModel have to be added
        // In 'AppShell.xaml' the 'ShellContent' for each page has to be added 
        // In 'AppShell.xaml.cs' the Navigation routes have to be registered
        // 
        // For Windows the initial Windowsize and -position are set in MauiProgam.cs
        // or can be set in 'App.xaml.cs in an override

        private readonly MainPageViewModel vm;
        public MainPage(MainPageViewModel viewModel)
        {
            InitializeComponent();
            vm = viewModel;
            BindingContext = vm;
        }   
    }
}
