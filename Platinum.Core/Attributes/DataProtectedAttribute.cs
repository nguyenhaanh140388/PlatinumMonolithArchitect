using System;
using System.Collections.Generic;
using System.Text;

namespace Anhny010920.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public class DataProtectedAttribute : Attribute
    {
    }
}
