using Android.App;
using Android.Content;

namespace Przypominajka.Platforms.Android
{
    [BroadcastReceiver(Enabled = true, Exported = true)]
    [IntentFilter(new[] {Intent.ActionAirplaneModeChanged }) ]
    public class BootReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context? context, Intent? intent)
        {
            if (intent.Action == Intent.ActionBootCompleted && context != null)
            {
                Task.Run(async () => await AlarmService.ScheduleAllAlarms()).Wait();
            }
        }
    }
}
