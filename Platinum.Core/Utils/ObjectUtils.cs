using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Platinum.Core.Utils
{
    public static class ObjectUtils
    {  /// <summary>
       /// Compares the object.
       /// </summary>
       /// <param name="self">The self.</param>
       /// <param name="to">To.</param>
       /// <param name="differenceProperties">The difference properties.</param>
       /// <param name="ignore">The ignore.</param>
       /// <returns>Result.</returns>
        public static bool CompareObject(object self, object to, out Dictionary<string, string> differenceProperties, params string[] ignore)
        {
            differenceProperties = new Dictionary<string, string>();

            if (self != null && to != null)
            {
                Type type = self.GetType();
                List<string> ignoreList = new List<string>(ignore);
                foreach (PropertyInfo pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (!ignoreList.Contains(pi.Name))
                    {
                        object selfValue = type.GetProperty(pi.Name).GetValue(self, null);
                        object toValue = type.GetProperty(pi.Name).GetValue(to, null);

                        if (selfValue != toValue && (selfValue == null || !selfValue.Equals(toValue)))
                        {
                            differenceProperties.Add(pi.Name, $"Current values: {toValue}");
                        }
                    }
                }

                return differenceProperties.Count == 0;
            }

            return self == to;
        }

    }
}
