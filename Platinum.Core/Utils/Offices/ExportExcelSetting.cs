namespace Platinum.Core.Utils.Offices
{
    using System;

    /// <summary>
    /// ImportExcelSetting.
    /// </summary>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public class ExportExcelSetting : Attribute
    {
        /// <summary>
        /// Gets or sets the header namme.
        /// </summary>
        /// <value>
        /// The header namme.
        /// </value>
        public string DisplayName { get; set; }

        public int Position { get; set; }

        public bool IsNumber { get; set; }

        public bool IsDate { get; set; }

        public string FormatString { get; set; }
    }

    public class NumberFormat
    {
        public NumberFormat(bool isNumber, string formatString)
        {
            IsNumber = isNumber;
            FormatString = formatString;
        }

        public bool IsNumber { get; set; }
        public string FormatString { get; set; }
    }

    public enum NumberType
    {
        NumberBasic,
        Accounting,
    }
}
