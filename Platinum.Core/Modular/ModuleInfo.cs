using System.Reflection;

namespace Platinum.Core.Modular
{
    public class ModuleInfo
    {
        public string Name { get; set; }

        public Assembly Assembly { get; set; }

        public string WebRootPath { get; set; }

        public string ShortName => Name.Split('.').Last();

        public string Path { get; set; }
    }
}
