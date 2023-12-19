namespace Platinum.Core.Utils.Offices
{
    /// <summary>
    /// ExcelImportConfig.
    /// </summary>
    public class ExcelExportConfig
    {
        /// <summary>
        /// Gets or sets the file.
        /// </summary>
        /// <value>
        /// The file.
        /// </value>
        public string FilePath { get; set; }

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
        public string SheetName { get; set; }

        /// <summary>
        /// Gets or sets the row data position.
        /// </summary>
        /// <value>
        /// The row data position.
        /// </value>
        public int RowDataPosition { get; set; }

        public bool AutoFit { get; set; }
    }
}
