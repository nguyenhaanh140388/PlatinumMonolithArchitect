namespace Platinum.Core.Models
{
    public class Sort : ISort
    {
        public string Prop { get; set; }
        public bool IsDesc { get; set; }
        public int Priority { get; set; }
    }
}
