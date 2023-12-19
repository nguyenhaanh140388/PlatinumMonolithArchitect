namespace Platinum.Core.Models
{
    public interface ISort
    {
        string Prop { get; set; }
        bool IsDesc { get; set; }
        int Priority { get; set; }
    }
}
