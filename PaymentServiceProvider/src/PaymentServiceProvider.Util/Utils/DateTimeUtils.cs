using System.Globalization;

namespace PaymentServiceProvider.Util.Utils
{
    public static class DateTimeUtils
    {
        public static string DateFormatter(this DateTime value)
        {
            return value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        }

        public static DateTime ParseToDate(this string value)
        {
            _ = DateTime.TryParse(value, out DateTime result);

            return result.Date;
        }
    }
}
