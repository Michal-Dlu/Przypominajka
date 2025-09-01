using Przypominajka.Models;
using Przypominajka.Services;
#if ANDROID
using Przypominajka.Platforms.Android;
#endif

namespace Przypominajka;
public partial class MainPage : ContentPage
{
    private Timer? _midnightTimer;
    private bool isInitialized = false;
    private PermissionStatus status;
    private readonly INotificationManagerService _notificationService;

    public MainPage(INotificationManagerService notificationService)
    {
        InitializeComponent();
        _notificationService = notificationService;
        StartMidnightWatcher();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await InitializeAsync();
        await Lista();
#if ANDROID
        await AlarmService.ScheduleAllAlarms();

        await RequestNotificationPermissionIfNeeded();
#endif
    }
    
    private async Task InitializeAsync()
    {
        if (!isInitialized)
            await Lista();
        isInitialized = true;
#if ANDROID
            await AlarmService.ScheduleAllAlarms();
#endif
         // await SendMessage();
    }
    private void StartMidnightWatcher()
    {
        var now = DateTime.Now;
        var midnight = now.Date.AddDays(1);
        var timeToMidnight = midnight - now;

        _midnightTimer = new Timer(async _ =>
        {
           #if ANDROID
            await AlarmService.ScheduleAllAlarms();
           #endif

            _midnightTimer?.Change(TimeSpan.FromHours(24), Timeout.InfiniteTimeSpan);
        }, null, timeToMidnight, Timeout.InfiniteTimeSpan);
    }

    private async void OnNavigateButtonClicked(object sender, EventArgs e)
    { 
        await Navigation.PushAsync(new Form());
        //var appShell = (AppShell)Shell.Current;
        //await appShell.PrzejdzDoStrony2();
    }

    private async Task RequestNotificationPermissionIfNeeded()
    {
        if (OperatingSystem.IsAndroidVersionAtLeast(33))
        {
#if ANDROID
            status = await Permissions.RequestAsync<NotificationManagerService.NotificationPermission>();
            if (status != PermissionStatus.Granted)
            {
                await DisplayAlert("Uwaga", "Nie zezwolono na powiadomienia. Aplikacja może nie przypominać o lekach.", "OK");
            }
#endif
        }
        else if (OperatingSystem.IsIOS())
        {
            var status = await Permissions.RequestAsync<Permissions.PostNotifications>();
            if (status != PermissionStatus.Granted)
            {
                await DisplayAlert("Uwaga", "Nie zezwolono na powiadomienia. Aplikacja może nie przypominać o lekach.", "OK");
            }
        }
        else if (OperatingSystem.IsMacCatalyst())
        {
            var status = await Permissions.RequestAsync<Permissions.PostNotifications>();
            if (status != PermissionStatus.Granted)
            {
                await DisplayAlert("Uwaga", "Nie zezwolono na powiadomienia. Aplikacja może nie przypominać o lekach.", "OK");
            }
        }
        else
        {
            if (OperatingSystem.IsAndroid())
            {
                await DisplayAlert("Informacja", "Powiadomienia są włączone domyślnie dla tej wersji Androida.", "OK");
            }
        }
    }

    private async Task Lista()
    {
        DatabaseService databaseService = new DatabaseService("leki.db");

        // Tworzenie instancji serwisu bazy danych
        List<Lek> leki = await databaseService.PobierzLek();  // Pobranie danych z bazy
        List<GodzinyPodania> godzinyPodania = await databaseService.PobierzGodzinyPodania();  // Pobranie godzin podania
        List<LekGodzina> lekiGodziny = new List<LekGodzina>();

        foreach (var lek in leki)
        {

            var godzinyDlaLeku = godzinyPodania
         .Where(g => g.LekId == lek.Id)
         .Select(g => g.godzina)  // Pobieramy tylko godziny

         .OrderBy(g => g)
         .Select(g => g.ToString() + ":00")
         .ToList();
            lekiGodziny.Add(new LekGodzina
            {
                nazwa = lek.nazwa,
                godziny = godzinyDlaLeku

            });// Każdy lek ma swoją unikalną listę godzin
        }
        LekiLabel.ItemsSource = lekiGodziny;  // Użycie LekiLabel z XAML
    }

   
}



