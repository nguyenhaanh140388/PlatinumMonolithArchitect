using System;
using System.Collections.Generic;
using System.Text;

namespace Anhny010920.Core.Modular
{
    public static class GlobalConfiguration
    {
        static GlobalConfiguration()
        {
            Modules = new List<ModuleInfo>();
        }

        public static IList<ModuleInfo> Modules { get; set; }
    }
}
