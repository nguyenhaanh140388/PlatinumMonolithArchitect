using System.Text;

namespace Anhny010920.Core.Extensions
{
    /// <summary>
    /// Provides extensions for <see cref="StringBuilder"/>
    /// </summary>
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// Appends <paramref name="value"/> to a StringBuilder only if <paramref name="condition"/> is met.
        /// </summary>
        public static StringBuilder AppendIf(this StringBuilder builder, bool condition, string value)
        {
            if (condition)
            {
                builder.Append(value);
            }

            return builder;
        }

        /// <summary>
        /// Appends <paramref name="value"/> to a StringBuilder only if <paramref name="condition"/> is met.
        /// </summary>
        public static StringBuilder AppendIf(this StringBuilder builder, bool condition, int value)
        {
            if (condition)
            {
                builder.Append(value);
            }

            return builder;
        }

        /// <summary>
        /// Appends <paramref name="value"/> to a StringBuilder only if <paramref name="condition"/> is met.
        /// </summary>
        public static StringBuilder AppendIf(this StringBuilder builder, bool condition, bool value)
        {
            if (condition)
            {
                builder.Append(value);
            }

            return builder;
        }

        /// <summary>
        /// Appends <paramref name="value"/> to a StringBuilder only if <paramref name="condition"/> is met.
        /// </summary>
        public static StringBuilder AppendIf(this StringBuilder builder, bool condition, char value)
        {
            if (condition)
            {
                builder.Append(value);
            }

            return builder;
        }

        /// <summary>
        /// Appends <paramref name="value"/> to a StringBuilder only if <paramref name="condition"/> is met.
        /// </summary>
        public static StringBuilder AppendIf(this StringBuilder builder, bool condition, sbyte value)
        {
            if (condition)
            {
                builder.Append(value);
            }

            return builder;
        }

        /// <summary>
        /// Appends <paramref name="value"/> to a StringBuilder only if <paramref name="condition"/> is met.
        /// </summary>
        public static StringBuilder AppendIf(this StringBuilder builder, bool condition, byte value)
        {
            if (condition)
            {
                builder.Append(value);
            }

            return builder;
        }

        /// <summary>
        /// Appends <paramref name="value"/> to a StringBuilder only if <paramref name="condition"/> is met.
        /// </summary>
        public static StringBuilder AppendIf(this StringBuilder builder, bool condition, short value)
        {
            if (condition)
            {
                builder.Append(value);
            }

            return builder;
        }

        /// <summary>
        /// Appends <paramref name="value"/> to a StringBuilder only if <paramref name="condition"/> is met.
        /// </summary>
        public static StringBuilder AppendIf(this StringBuilder builder, bool condition, long value)
        {
            if (condition)
            {
                builder.Append(value);
            }

            return builder;
        }

        /// <summary>
        /// Appends <paramref name="value"/> to a StringBuilder only if <paramref name="condition"/> is met.
        /// </summary>
        public static StringBuilder AppendIf(this StringBuilder builder, bool condition, float value)
        {
            if (condition)
            {
                builder.Append(value);
            }

            return builder;
        }

        /// <summary>
        /// Appends <paramref name="value"/> to a StringBuilder only if <paramref name="condition"/> is met.
        /// </summary>
        public static StringBuilder AppendIf(this StringBuilder builder, bool condition, double value)
        {
            if (condition)
            {
                builder.Append(value);
            }

            return builder;
        }

        /// <summary>
        /// Appends <paramref name="value"/> to a StringBuilder only if <paramref name="condition"/> is met.
        /// </summary>
        public static StringBuilder AppendIf(this StringBuilder builder, bool condition, decimal value)
        {
            if (condition)
            {
                builder.Append(value);
            }

            return builder;
        }

        /// <summary>
        /// AppendFormats to a StringBuilder only if <paramref name="condition"/> is met.
        /// </summary>
        public static StringBuilder AppendFormatIf(this StringBuilder builder, bool condition, string format, object arg0)
        {
            if (condition)
            {
                builder.AppendFormat(format, arg0);
            }

            return builder;
        }

        /// <summary>
        /// AppendFormats to a StringBuilder only if <paramref name="condition"/> is met.
        /// </summary>
        public static StringBuilder AppendFormatIf(this StringBuilder builder, bool condition, string format, object arg0, object arg1)
        {
            if (condition)
            {
                builder.AppendFormat(format, arg0, arg1);
            }

            return builder;
        }

        /// <summary>
        /// AppendFormats to a StringBuilder only if <paramref name="condition"/> is met.
        /// </summary>
        public static StringBuilder AppendFormatIf(this StringBuilder builder, bool condition, string format, object arg0, object arg1, object arg2)
        {
            if (condition)
            {
                builder.AppendFormat(format, arg0, arg1, arg2);
            }

            return builder;
        }

        /// <summary>
        /// AppendFormats to a StringBuilder only if <paramref name="condition"/> is met.
        /// </summary>
        public static StringBuilder AppendFormatIf(this StringBuilder builder, bool condition, string format, params object[] args)
        {
            if (condition)
            {
                builder.AppendFormat(format, args);
            }

            return builder;
        }

        /// <summary>
        /// AppendLines to a StringBuilder only if <paramref name="condition"/> is met.
        /// </summary>
        public static StringBuilder AppendLineIf(this StringBuilder builder, bool condition)
        {
            if (condition)
            {
                builder.AppendLine();
            }

            return builder;
        }

        /// <summary>
        /// AppendLines <paramref name="value"/> to a StringBuilder only if <paramref name="condition"/> is met.
        /// </summary>
        public static StringBuilder AppendLineIf(this StringBuilder builder, bool condition, string value)
        {
            if (condition)
            {
                builder.AppendLine(value);
            }

            return builder;
        }
    }
}