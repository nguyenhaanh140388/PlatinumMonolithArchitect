namespace Platinum.Core.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IFilter
    {
        string Prop { get; set; }
        object Val { get; set; }
        string Operator { get; set; }
    }
}
