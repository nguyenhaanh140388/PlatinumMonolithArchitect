namespace Platinum.Core.Extensions
{
    /// <summary>
    /// Provides extensions for <see cref="double"/>
    /// </summary>
    public static class DoubleExtensions
    {
        /// <summary>
        /// Converts the value from a UNIX Timestamp to a DateTime.
        /// </summary>
        public static DateTime AsDateTimeFromUnixTimestamp(this double input)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0)
                .AddSeconds(input);
        }
    }
}