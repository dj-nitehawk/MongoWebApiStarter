using NodaTime;
using NodaTime.Text;

namespace MongoWebApiStarter;

/// <summary>
/// Utility for converting local DateTime to UTC and vise versa
/// </summary>
public static class Dates
{
    public const string year_month_date = "yyyy-MM-dd";
    public const string hour_minute = "HH:mm";
    public const string default_timezone = "Asia/Colombo"; // all zones: https://nodatime.org/TimeZones

    /// <summary>
    /// Returns the local date segment "2020-12-31" from a UTC DateTime instance for a given time zone
    /// </summary>
    /// <param name="UTCDateTime">The input UTC DateTime</param>
    /// <param name="timeZone">The time zone to convert the DateTime in to</param>
    public static string ToDatePart(this DateTime UTCDateTime, string timeZone = default_timezone)
    {
        return UTCDateTime == default
            ? throw new ArgumentException("Cannot convert default dates to local dates!")
            : ToLocal(UTCDateTime, timeZone).ToString(year_month_date);
    }

    /// <summary>
    /// Returns the local time segment "12:30 AM" from a UTC DateTime instance for a given time zone
    /// </summary>
    /// <param name="UTCDateTime">The input UTC DateTime</param>
    /// <param name="timeZone">The time zone to convert the DateTime in to</param>
    public static string ToTimePart(this DateTime UTCDateTime, string timeZone = default_timezone)
    {
        return UTCDateTime == default
            ? throw new ArgumentException("Cannot convert default dates to local dates!")
            : ToLocal(UTCDateTime, timeZone).ToString(hour_minute);
    }

    /// <summary>
    /// Converts a UTC DateTime to the given local time zone
    /// </summary>
    /// <param name="UTCDateTime">The input UTC DateTime</param>
    /// <param name="timeZone">The time zone to convert the DateTime in to</param>
    public static DateTime ToLocal(this DateTime UTCDateTime, string timeZone = default_timezone)
    {
        return UTCDateTime == default
            ? throw new ArgumentException("Cannot convert default dates to local dates!")
            : UTCDateTime.Kind != DateTimeKind.Utc
                ? throw new ArgumentException("The supplied date must be a UTC date/time!")
                : Instant.FromDateTimeUtc(UTCDateTime)
                         .InZone(DateTimeZoneProviders.Tzdb[timeZone])
                         .ToDateTimeUnspecified();
    }

    /// <summary>
    /// Create a UTC DateTime from given date and time strings and the time zone
    /// </summary>
    /// <param name="date">Local date string "2020-12-31"</param>
    /// <param name="time">Local time string "12:12 AM"</param>
    /// <param name="timeZone">The time zone of the local date/time</param>
    public static DateTime ToUTC(string date, string time = "00:00", string timeZone = default_timezone)
    {
        var result = LocalDateTimePattern
            .CreateWithInvariantCulture($"{year_month_date} {hour_minute}")
            .Parse(date + " " + time);

        return !result.Success
            ? throw new InvalidPatternException(result.Exception.Message)
            : result.Value
                    .InZoneStrictly(DateTimeZoneProviders.Tzdb[timeZone])
                    .ToDateTimeUtc();
    }

    /// <summary>
    /// Returns the first day of the week for a given datetime.
    /// The time is set to midnight (00:00:00)
    /// </summary>
    public static DateTime FirstDayOfWeek(this DateTime dayInWeek)
    {
        const DayOfWeek firstDay = DayOfWeek.Sunday;
        DateTime firstDayInWeek = dayInWeek.Date;
        while (firstDayInWeek.DayOfWeek != firstDay)
            firstDayInWeek = firstDayInWeek.AddDays(-1);

        return firstDayInWeek;
    }

    /// <summary>
    /// Check if a date and time string format is correct.
    /// </summary>
    /// <param name="date">Local date string "2020-12-31"</param>
    /// <param name="time">Local time string "12:12 AM"</param>
    public static bool DateTimeFormatIsCorrect(string date, string time = "00:00")
    {
        var result = LocalDateTimePattern
            .CreateWithInvariantCulture($"{year_month_date} {hour_minute}")
            .Parse(date + " " + time);

        return result.Success;
    }
}