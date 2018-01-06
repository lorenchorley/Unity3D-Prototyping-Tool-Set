using System;

public static class DateExtensions {

    public static bool Between(this DateTime dt, DateTime rangeBeg, DateTime rangeEnd) {
        return dt.Ticks >= rangeBeg.Ticks && dt.Ticks <= rangeEnd.Ticks;
    }

    public static int CalculateAge(this DateTime dateTime) {
        var age = DateTime.Now.Year - dateTime.Year;
        if (DateTime.Now < dateTime.AddYears(age))
            age--;
        return age;
    }

    public static string ToReadableTime(this DateTime value) {
        var ts = new TimeSpan(DateTime.UtcNow.Ticks - value.Ticks);
        double delta = ts.TotalSeconds;
        if (delta < 60) {
            return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";
        }
        if (delta < 120) {
            return "a minute ago";
        }
        if (delta < 2700) // 45 * 60
        {
            return ts.Minutes + " minutes ago";
        }
        if (delta < 5400) // 90 * 60
        {
            return "an hour ago";
        }
        if (delta < 86400) // 24 * 60 * 60
        {
            return ts.Hours + " hours ago";
        }
        if (delta < 172800) // 48 * 60 * 60
        {
            return "yesterday";
        }
        if (delta < 2592000) // 30 * 24 * 60 * 60
        {
            return ts.Days + " days ago";
        }
        if (delta < 31104000) // 12 * 30 * 24 * 60 * 60
        {
            int months = Convert.ToInt32(Math.Floor((double) ts.Days / 30));
            return months <= 1 ? "one month ago" : months + " months ago";
        }
        var years = Convert.ToInt32(Math.Floor((double) ts.Days / 365));
        return years <= 1 ? "one year ago" : years + " years ago";
    }

    public static bool WorkingDay(this DateTime date) {
        return date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;
    }

    public static bool IsWeekend(this DateTime date) {
        return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
    }

    public static DateTime NextWorkday(this DateTime date) {
        var nextDay = date;
        while (!nextDay.WorkingDay()) {
            nextDay = nextDay.AddDays(1);
        }
        return nextDay;
    }

}