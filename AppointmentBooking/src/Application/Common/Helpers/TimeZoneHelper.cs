namespace AppointmentBooking.src.Application.Common.Helpers;

public static class TimeZoneHelper
{
    private static readonly TimeZoneInfo GeorgiaTz =
        TimeZoneInfo.FindSystemTimeZoneById("Georgian Standard Time");

    public static DateTime ConvertToGeorgian(DateTime utcDateTime)
    {
        return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, GeorgiaTz);
    }

    public static DateTime ConvertToUtcFromGeorgian(DateTime georgianDateTime)
    {
        return TimeZoneInfo.ConvertTimeToUtc(georgianDateTime, GeorgiaTz);
    }
}
