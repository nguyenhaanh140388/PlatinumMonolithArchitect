namespace Platinum.Core.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface ISort
    {
        string Prop { get; set; }
        bool IsDesc { get; set; }
        int Priority { get; set; }
    }
}
