namespace SalesAPI.Application.Helpers
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Ensures that the DateTime is in UTC.
        /// If the DateTime is already in UTC, it returns the same DateTime.
        /// If the DateTime is in Local or Unspecified, it converts it to UTC.
        /// </summary>
        /// <param name="dateTime">The DateTime to convert to UTC.</param>
        /// <returns>A DateTime in UTC.</returns>
        public static DateTime? EnsureUtc(this DateTime? dateTime)
        {
            if (dateTime == null) return dateTime;
            if (dateTime.Value.Kind == DateTimeKind.Utc)
            {
                return dateTime;
            }
            return DateTime.SpecifyKind(dateTime.Value, DateTimeKind.Utc).ToUniversalTime();
        }
        public static DateTime EnsureUtc(this DateTime dateTime)
        {

            return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc).ToUniversalTime();
        }
    }
}
