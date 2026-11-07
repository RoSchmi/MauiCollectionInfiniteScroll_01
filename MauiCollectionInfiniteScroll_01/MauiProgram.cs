using MauiCollectionInfiniteScroll_01.ViewModels;

using Microsoft.Extensions.Logging;

namespace MauiCollectionInfiniteScroll_01
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("JetBrainsMono-Regular.ttf", "JetBrainsMono");
                    fonts.AddFont("JetBrainsMono-Bold.ttf", "JetBrainsMonoBold");

                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<ViewModels.MainPageViewModel>();
            builder.Services.AddTransient<Pages.DisplayPage>();       
        
            return builder.Build();
        }
    }
}
