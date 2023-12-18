using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Reflection;

namespace Anhny010920.Core.Utilities
{
    public static class EmbeddedResourceUtils
    {
        /// <summary>
        /// Get embedded resource by short name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>The resource as stream if there is only 1 resource whose fullname ends with provided name. Null if no match. Throws Argument Exception if there are more than 1 matches</returns>
        public static Stream GetStream(string name, Assembly assembly = null)
        {
            if (assembly == null)
            {
                var assemblies = ReflectionUtils.GetAssemblies();
                foreach (var assem in assemblies)
                {
                    //Skip dynamic assemblies
                    if (assem.IsDynamic)
                    {
                        continue;
                    }
                    var resourceNames = assem.GetManifestResourceNames().Where(r => r.Equals(name)).ToList();
                    if (resourceNames.Count > 1)
                    {
                        throw new ArgumentException(string.Format("Ambigous resource name: {0}", resourceNames.First()));
                    }
                    if (resourceNames.Any())
                    {
                        return assem.GetManifestResourceStream(resourceNames.First());
                    }
                }
            }
            else
            {
                var resourceNames = assembly.GetManifestResourceNames().Where(r => r.Equals(name)).ToList();
                if (resourceNames.Count > 1)
                {
                    throw new ArgumentException(string.Format("Ambigous resource name: {0}", resourceNames.First()));
                }
                if (resourceNames.Any())
                {
                    return assembly.GetManifestResourceStream(resourceNames.First());
                }
            }

            throw new ArgumentException(string.Format("Missing resource name: {0}", name));
        }



        /// <summary>
        /// Get embedded resource by short name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="namespace"></param>
        /// <param name="assembly">The assembly to look for resource, the calling assembly will be use if null is passed</param>
        /// <returns>The resource as string if there is only 1 resource whose fullname ends with provided name. Null if no match. Throws Argument Exception if there are more than 1 matches</returns>
        public static string GetString(string name, string @namespace, Assembly assembly = null)
        {
            if (!string.IsNullOrEmpty(@namespace))
            {
                name = string.Format("{0}.{1}", @namespace, name);
            }
            var stream = GetStream(name, assembly);
            using (var reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                return result;
            }
        }



        /// <summary>
        /// Get embedded resource by short name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="namespace"></param>
        /// <param name="assembly">The assembly to look for resource, the calling assembly will be use if null is passed</param>
        /// <returns>The resource as string if there is only 1 resource whose fullname ends with provided name. Null if no match. Throws Argument Exception if there are more than 1 matches</returns>
        public static string GetNullableString(string name, string @namespace, Assembly assembly = null)
        {
            try
            {
                return GetString(name, @namespace, assembly);
            }
            catch (System.Exception)
            {
                //If cannot find the resource then return empty string
                return string.Empty;
            }
        }
    }
}
