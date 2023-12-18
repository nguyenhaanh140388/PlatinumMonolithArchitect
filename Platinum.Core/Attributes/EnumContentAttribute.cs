using System;
using System.Collections.Generic;
using System.Text;

namespace Platinum.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class EnumContentAttribute : Attribute
    {
        public string Key { get; private set; }
        public string Value { get; private set; }

        public EnumContentAttribute(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
