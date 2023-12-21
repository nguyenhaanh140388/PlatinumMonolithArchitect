using Platinum.Core.Abstractions.Models.Response;
using Platinum.Core.Utils.Offices;

namespace Platinum.Core.Models
{
    /// <summary>
    /// ResultImportViewModel.
    /// </summary>
    /// <seealso cref="Platinum.Entities.Models.OperationResult" />
    public class ResultImportResponseResult : OperationResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResultImportResponseResult" /> class.
        /// </summary>
        /// <param name="isSuccess">if set to <c>true</c> [is success].</param>
        public ResultImportResponseResult(bool isSuccess)
            : base(isSuccess)
        {
        }

        /// <summary>
        /// Gets or sets the error validation information.
        /// </summary>
        /// <value>
        /// The error validation information.
        /// </value>
        public IEnumerable<ErrorValidationInfo> ErrorValidationInfo { get; set; }

        /// <summary>
        /// Gets or sets the transaction result.
        /// </summary>
        /// <value>
        /// The transaction result.
        /// </value>
        public ITransactionResult TransactionResult { get; set; }
    }
}
