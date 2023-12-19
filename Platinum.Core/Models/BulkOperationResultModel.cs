using Z.BulkOperations;

namespace Platinum.Core.Models
{
    public class BulkOperationResultModel
    {
        public ResultInfo ResultInfo { get; set; }
        public List<AuditEntry> AuditEntries { get; set; }
    }
}
