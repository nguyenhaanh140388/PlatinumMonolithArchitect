namespace Platinum.Core.Models
{
    public class MergingOutputModel
    {
        public string Action { get; set; }
        public Guid TargetId { get; set; }
        public DateTime TargetModifiedDate { get; set; }
        public string TargetName { get; set; }
        public Guid SourceId { get; set; }
        public DateTime SourceModifiedDate { get; set; }
        public string SourceName { get; set; }
    }
}
