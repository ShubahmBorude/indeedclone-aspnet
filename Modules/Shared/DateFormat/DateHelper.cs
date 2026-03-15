namespace IndeedClone.Modules.Shared.DateFormat
{
    public static class DateHelper
    {
        private const string IndianTimeZoneId = "India Standard Time";

        // # Conversion logic (generic)
        private static DateTime ConvertToTimeZone(DateTime utcDate, string timeZoneId)
        {
            TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return TimeZoneInfo.ConvertTimeFromUtc(utcDate, tz);
        }

        // # IST timestamp
        public static DateTime IST_Date()
        {
            return ConvertToTimeZone(DateTime.UtcNow, IndianTimeZoneId);
        }

        // # Date Time Formatting (optional)
        public static string Atc_DateString(string format = "dd-MMM-yyyy HH:mm:ss")
        {
            DateTime istTime = IST_Date();
            return istTime.ToString(format);
        }

    }
}