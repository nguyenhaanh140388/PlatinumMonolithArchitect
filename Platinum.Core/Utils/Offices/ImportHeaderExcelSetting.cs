namespace Platinum.Core.Utils.Offices
{
    using System;

    /// <summary>
    /// ImportExcelSetting.
    /// </summary>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public class ImportExcelSetting : Attribute
    {
        /// <summary>
        /// Gets or sets the header position.
        /// </summary>
        /// <value>
        /// The header position.
        /// </value>
        public int Position { get; set; }

        /// <summary>
        /// Gets or sets the header namme.
        /// </summary>
        /// <value>
        /// The header namme.
        /// </value>
        public string HeaderName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is read text.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is read text; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadText { get; set; }
    }
}
