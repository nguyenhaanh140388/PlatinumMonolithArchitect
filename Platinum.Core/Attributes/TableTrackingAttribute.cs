using System;
using System.Collections.Generic;
using System.Text;

namespace Platinum.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class TableTrackingAttribute : Attribute
    {
    }
}
