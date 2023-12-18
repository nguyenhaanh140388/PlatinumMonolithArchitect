using System;
using System.Collections.Generic;
using System.Text;

namespace Platinum.Core.Modular
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
