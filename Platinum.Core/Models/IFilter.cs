namespace Platinum.Core.Models
{
    public interface IFilter
    {
        string Prop { get; set; }
        object Val { get; set; }
        string Operator { get; set; }
    }
}
