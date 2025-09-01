using Android.App;
using Android.Content;

namespace Przypominajka.Platforms.Android
{
    public static class AlarmScheduler
    {
        public static void ScheduleExactAlarm(Context context, DateTime dateTime, int requestCode, string lekiString)
        {
            Intent intent = new Intent(context, typeof(MidnightAlarmReceiver));
            intent.SetAction("PRZYPOMINAJKA.EXACT_ALARM");
            intent.PutExtra("leki", lekiString);  // Przekazujemy nazwy leków do powiadomienia

            PendingIntent pendingIntent = PendingIntent.GetBroadcast(context, requestCode, intent, PendingIntentFlags.Immutable | PendingIntentFlags.UpdateCurrent);
            AlarmManager alarmManager = (AlarmManager)context.GetSystemService(Context.AlarmService);

            // Konwertujemy DateTime na milisekundy od epoki
            long triggerTime = (long)(dateTime.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds;

            var alarmInfo = new AlarmManager.AlarmClockInfo(triggerTime, pendingIntent);

            alarmManager.SetAlarmClock(alarmInfo, pendingIntent);


        }
    }
}
