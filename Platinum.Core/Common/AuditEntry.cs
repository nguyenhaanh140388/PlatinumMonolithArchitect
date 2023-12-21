using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using Platinum.Core.Entities;
using Platinum.Core.Enums;

namespace Platinum.Core.Common
{
    public class AuditEntry
    {
        public AuditEntry(EntityEntry entry)
        {
            Entry = entry;
        }
        public EntityEntry Entry { get; }
        public Guid  UserId { get; set; }
        public string TableName { get; set; }
        public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();
        public AuditType AuditType { get; set; }
        public List<string> ChangedColumns { get; } = new List<string>();
        public Audit ToAudit()
        {
            var audit = new Audit
            {
                CreatedBy = UserId,
                ModifiedBy = UserId,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Type = AuditType.ToString(),
                TableName = TableName,
                PrimaryKey = JsonConvert.SerializeObject(KeyValues),
                OldValues = OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues)!,
                NewValues = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues)!,
                AffectedColumns = ChangedColumns.Count == 0 ? null : JsonConvert.SerializeObject(ChangedColumns)!
            };
            return audit;
        }
    }
}
