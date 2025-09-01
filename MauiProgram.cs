using Microsoft.Extensions.Logging;
using Przypominajka.Services;

namespace Przypominajka
{
    public static class MauiProgram
    {

        public static MauiApp CreateMauiApp()
        {
            SQLitePCL.Batteries.Init();
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddSingleton<DatabaseService>(serviceProvider =>
            {
                // Określenie ścieżki do bazy danych
                var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "mydatabase.db");
                return new DatabaseService(databasePath);
            });
            builder.Services.AddTransient<MainPage>();
#if __ANDROID__
            Console.WriteLine("Rejestruję NotificationManagerService");
            builder.Services.AddSingleton<INotificationManagerService, Przypominajka.Platforms.Android.NotificationManagerService>();
#endif

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            
            return builder.Build();
        }
    }
}

