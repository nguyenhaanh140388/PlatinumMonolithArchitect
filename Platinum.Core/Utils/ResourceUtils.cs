﻿namespace Platinum.Core.Utils
{
    public static class ResourceUtils
    {
        public static string GetResourceContent(string name, string resourceFolder, string resourceNamespace)
        {
            return EmbeddedResourceUtils.GetString(name, string.Format("{0}.{1}", resourceNamespace, resourceFolder));
        }
    }
}
