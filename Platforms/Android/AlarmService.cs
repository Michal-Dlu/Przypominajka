using Przypominajka.Models;
using Przypominajka.Services;
using AndroidApp = Android.App.Application;

namespace Przypominajka.Platforms.Android
{
    public static class AlarmService
    {
        public static async Task ScheduleAllAlarms()
        {
            DatabaseService databaseService = new DatabaseService("leki.db");
            List<Lek> leki = await databaseService.PobierzLek();
            List<GodzinyPodania> godzinyPodania = await databaseService.PobierzGodzinyPodania();

            var context = AndroidApp.Context;
            int requestCode = 0;
            var teraz = DateTime.Now;

            for (int i = 0; i < 24; i++)
            {
                var godzinyDlaLeku = godzinyPodania
                    .Where(g => g.godzina == i).ToList();
                if (godzinyDlaLeku.Any())
                {
                    var lekiDoPowiadomienia = from g in godzinyDlaLeku
                                              join l in leki on g.LekId equals l.Id
                                              select l.nazwa;

                    string lekiString = string.Join(",", lekiDoPowiadomienia);

                    DateTime alarmTime = new DateTime(teraz.Year, teraz.Month, teraz.Day, i, 0, 0);

                    if (alarmTime < teraz)
                    {
                        alarmTime = alarmTime.AddDays(1);
                    }
                    AlarmScheduler.ScheduleExactAlarm(context, alarmTime, requestCode++, lekiString);
                }
            }
        }
    }
}
