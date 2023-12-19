namespace Platinum.Core.Utils.Offices
{
    using System.IO;

    /// <summary>
    /// ExcelImportConfig.
    /// </summary>
    public class ExcelImportConfig
    {
        /// <summary>
        /// Gets or sets the file.
        /// </summary>
        /// <value>
        /// The file.
        /// </value>
        public Stream File { get; set; }

        /// <summary>
        /// Gets or sets the header row position.
        /// </summary>
        /// <value>
        /// The header row position.
        /// </value>
        public int HeaderRowPosition { get; set; }

        /// <summary>
        /// Gets or sets the sheet position.
        /// </summary>
        /// <value>
        /// The sheet position.
        /// </value>
        public int SheetPosition { get; set; }

        /// <summary>
        /// Gets or sets the row data position.
        /// </summary>
        /// <value>
        /// The row data position.
        /// </value>
        public int RowDataPosition { get; set; }

        /// <summary>
        /// Gets or sets has header.
        /// </summary>
        /// <value>
        /// The file.
        /// </value>
        public bool HasHeader { get; set; }
    }
}
