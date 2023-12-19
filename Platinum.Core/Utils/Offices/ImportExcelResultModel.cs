namespace Platinum.Core.Utils.Offices
{
    using System.Collections.Generic;

    /// <summary>
    /// ImportExcelResultModel.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public class ImportExcelResultModel<TModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportExcelResultModel{T}" /> class.
        /// </summary>
        public ImportExcelResultModel()
        {
            IsTemplateValid = true;
            DataIsEmpty = false;
            ResultData = new List<TModel>();
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is template valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is template valid; otherwise, <c>false</c>.
        /// </value>
        public bool IsTemplateValid { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [data is empty].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [data is empty]; otherwise, <c>false</c>.
        /// </value>
        public bool DataIsEmpty { get; set; }

        /// <summary>
        /// Gets or sets the result data.
        /// </summary>
        /// <value>
        /// The result data.
        /// </value>
        public List<TModel> ResultData { get; set; }
    }
}
