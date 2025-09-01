namespace Przypominajka.Messages
{
    public class MidnightAlarmMessage
    {
        public string LekiString { get; }
        public MidnightAlarmMessage(string lekiString)
        {
            LekiString = lekiString;
        }
    }
}
