namespace Platinum.Core.Extensions
{
    /// <summary>
    /// An enumeration of the types of masking styles for the Mask() extension method
    /// of the string class.
    /// </summary>
    public enum StringMaskStyle
    {
        /// <summary>
        /// Masks all characters within the masking region, regardless of type.
        /// </summary>
        All,

        /// <summary>
        /// Masks only alphabetic and numeric characters within the masking region.
        /// </summary>
        AlphaNumericOnly,
    }
}