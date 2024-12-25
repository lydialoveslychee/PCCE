using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Storage;
using epj.ProgressBar.Maui;
using Microsoft.Extensions.Logging;
using PCCE.ViewModel;

namespace PCCE
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseProgressBar()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddSingleton<IFileSaver>(FileSaver.Default);
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<ItemPage>();
            builder.Services.AddSingleton<ItemPageAndroid>();
            builder.Services.AddSingleton<ItemViewModel>();
            builder.Services.AddSingleton<LookupPage>();
            builder.Services.AddSingleton<LookupPageAndroid>();
            builder.Services.AddSingleton<LookupViewModel>();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
