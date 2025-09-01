using Android.App;
using Android.Content;
using CommunityToolkit.Mvvm.Messaging;
using Przypominajka.Messages;


namespace Przypominajka.Platforms.Android
{
    [BroadcastReceiver(Enabled = true, Exported = true)]
    [IntentFilter(new[] { "PRZYPOMINAJKA.MIDNIGHT_ALARM" })]
    public class MidnightAlarmReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context? context, Intent? intent)
        {
            // Pobierz przekazane nazwy leków
            string lekiString = intent.GetStringExtra("leki") ?? "Czas na leki";
            NotificationManagerService.Instance.SendNotification("Przypomnienie o lekach", $"Czas na: {lekiString}");

            MainThread.BeginInvokeOnMainThread(() =>
            { WeakReferenceMessenger.Default.Send(new MidnightAlarmMessage(lekiString)); });
        }
    }
}
