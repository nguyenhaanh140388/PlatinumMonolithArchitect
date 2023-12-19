namespace Platinum.Core.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Sort : ISort
    {
        public string Prop { get; set; }
        public bool IsDesc { get; set; }
        public int Priority { get; set; }
    }
}
