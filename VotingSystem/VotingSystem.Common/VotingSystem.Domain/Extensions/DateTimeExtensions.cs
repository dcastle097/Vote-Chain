using System;

namespace VotingSystem.Domain.Extensions
{
    /// <summary>
    ///     Extensions for the <see cref="DateTime" /> class.
    /// </summary>
    public static class DateTimeExtensions
    {
        private static readonly DateTime Epoc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        ///     Convert a <see cref="DateTime" /> value to an <see cref="Int64" /> unix timestamp.
        /// </summary>
        /// <param name="value"><see cref="DateTime" /> to be converted.</param>
        /// <returns>An <see cref="Int64" /> unix timestamp</returns>
        public static long ToUnixTimestamp(this DateTime value)
        {
            return (long)value.ToUniversalTime().Subtract(Epoc).TotalMilliseconds;
        }

        /// <summary>
        ///     Convert an <see cref="Int64" /> value to a universal time <see cref="DateTime" />.
        /// </summary>
        /// <param name="value"><see cref="Int64" /> to be converted.</param>
        /// <returns>A <see cref="DateTime" /> in universal time format</returns>
        public static DateTime ToDateTime(this long value)
        {
            return Epoc.AddMilliseconds(value).ToUniversalTime();
        }
    }
}