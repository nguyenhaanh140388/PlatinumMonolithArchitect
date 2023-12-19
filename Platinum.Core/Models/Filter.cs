namespace Platinum.Core.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Filter
    {
        public string Prop { get; set; }
        public object Val { get; set; }
        public object Val2 { get; set; }
        public string Operator { get; set; }
    }
}
