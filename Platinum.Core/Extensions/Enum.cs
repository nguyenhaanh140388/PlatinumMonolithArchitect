using Platinum.Core.Extensions;
// ReSharper disable UnusedMember.Global

namespace Platinum.Core.Extensions
{
    /// <summary>
    /// Provides strongly typed methods for working with <see cref="Enum"/>.
    /// </summary>
    /// <remarks>
    /// Credit from http://damieng.com/blog/2010/10/17/enums-better-syntax-improved-performance-and-tryparse-in-net-3-5
    /// </remarks>
    public static class Enum<T> where T : struct
    {
        private static readonly IEnumerable<T> _all = Enum.GetValues(typeof(T)).Cast<T>();
        private static readonly Dictionary<string, T> _insensitiveNames = _all.ToDictionary(k => Enum.GetName(typeof(T), k)?.ToUpperInvariant());
        private static readonly Dictionary<string, T> _sensitiveNames = _all.ToDictionary(k => Enum.GetName(typeof(T), k));
        private static readonly Dictionary<int, T> _values = _all.ToDictionary(k => Convert.ToInt32(k));
        private static readonly Dictionary<T, string> _names = _all.ToDictionary(k => k, v => v.ToString());

        /// <summary>
        /// Gets whether or not the object provided is a valid entry in this enum.
        /// </summary>
        public static bool IsDefined(T value)
        {
            return _names.Keys.Contains(value);
        }

        /// <summary>
        /// Gets whether or not the string value provided is a valid entry in this enum.
        /// </summary>
        public static bool IsDefined(string value)
        {
            return _sensitiveNames.Keys.Contains(value);
        }

        /// <summary>
        /// Gets whether or not the integer value provided is a valid value within this enum.
        /// </summary>
        public static bool IsDefined(int value)
        {
            return _values.Keys.Contains(value);
        }

        /// <summary>
        /// Gets all possible enum values in this enum.
        /// </summary>
        public static IEnumerable<T> GetValues()
        {
            return _all;
        }

        /// <summary>
        /// Gets string representations of each value in this enum.
        /// </summary>
        public static string[] GetNames()
        {
            return _names.Values.ToArray();
        }

        /// <summary>
        /// Gets the string representation of a specific value in this enum.
        /// </summary>
        public static string GetName(T value)
        {
            _names.TryGetValue(value, out string name);
            return name;
        }

        /// <summary>
        /// Parses the string value into a enum.
        /// </summary>
        public static T Parse(string value)
        {
            if (!_sensitiveNames.TryGetValue(value, out T parsed))
            {
                throw new ArgumentException("Value is not one of the named constants defined for the enumeration", nameof(value));
            }

            return parsed;
        }

        /// <summary>
        /// Parses the string value into a enum, and allows you to specify whether to ignore casing.
        /// </summary>
        public static T Parse(string value, bool ignoreCase)
        {
            if (!ignoreCase)
            {
                return Parse(value);
            }

            if (!_insensitiveNames.TryGetValue(value.ToUpperInvariant(), out T parsed))
            {
                throw new ArgumentException("Value is not one of the named constants defined for the enumeration", nameof(value));
            }

            return parsed;
        }

        /// <summary>
        /// Tries to parses a specific enum value from a string.
        /// </summary>
        public static bool TryParse(string value, out T returnValue)
        {
            return _sensitiveNames.TryGetValue(value, out returnValue);
        }

        /// <summary>
        /// Tries to parses a specific enum value from a string, and allows you to specify whether to ignore casing.
        /// </summary>
        public static bool TryParse(string value, bool ignoreCase, out T returnValue)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                returnValue = default;
                return false;
            }

            return ignoreCase
                ? _insensitiveNames.TryGetValue(value.ToUpperInvariant(), out returnValue)
                : TryParse(value, out returnValue);
        }

        /// <summary>
        /// Parses the string value into a nullable enum. If unable to parse, will return null.
        /// </summary>
        public static T? ParseOrNull(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            if (_sensitiveNames.TryGetValue(value, out T foundValue))
            {
                return foundValue;
            }

            return null;
        }

        /// <summary>
        /// Parses the string value into a nullable enum, with the option to ignore casing. If unable to parse, will return null.
        /// </summary>
        public static T? ParseOrNull(string value, bool ignoreCase)
        {
            if (!ignoreCase)
            {
                return ParseOrNull(value);
            }

            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            if (_insensitiveNames.TryGetValue(value.ToUpperInvariant(), out T foundValue))
            {
                return foundValue;
            }

            return null;
        }

        /// <summary>
        /// Attempts to cast the enum integer value into an enum value. If unable to cast, returns null.
        /// </summary>
        public static T? CastOrNull(int value)
        {
            if (_values.TryGetValue(value, out T foundValue))
            {
                return foundValue;
            }

            return null;
        }

        /// <summary>
        /// Gets applied flags on an enum instance.
        /// </summary>
        public static IEnumerable<T> GetFlags(T flagEnum)
        {
            var flagInt = Convert.ToInt32(flagEnum);
            return _all.Where(e => (Convert.ToInt32(e) & flagInt) != 0);
        }

        /// <summary>
        /// Sets applied flags on an enum instance.
        /// </summary>
        public static T SetFlags(IEnumerable<T> flags)
        {
            var combined = flags.Aggregate(default(int), (current, flag) => current | Convert.ToInt32(flag));

            return _values.TryGetValue(combined, out T result)
                ? result
                : default;
        }
    }
}