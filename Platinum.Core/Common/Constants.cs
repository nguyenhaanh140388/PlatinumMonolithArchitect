namespace Platinum.Core.Common
{
    public static class Constants
    {
        public const string SolutionNameSpace = "Platinum";

        public static class ConnectionStringNames
        {
            public const string Administrator = "PlatinumIdentityDb";
            public const string Catalog = "PlatinumCatalog";
        }
        public static class KeyInfoProperties
        {
            public const string ConfigName = "KeyInfo";
        }

        public static class CorsProperties
        {
            public const string DefaultPolicy = "DefaultAppPolicy";
            public const string ConfigName = "Cors";
        }

        public static class DistributedSqlServerCache
        {
            public const string SchemaName = "dbo";
            public const string TableName = "DistributeCacheProductTable";
        }

        public static class SerilogProperties
        {
            public const string TableName = "Logs";
        }

        public static class EmailTemplateProperties
        {
            public const string FileName = "File";
            public const string EmailTemplateName = "EmailTemplate";
            public const string EmailSubject = "EmailSubject";
        }

        public static class ResourceNameSpaces
        {
            public const string PlatinumCoreResource = "Platinum.Core.Resources";
        }

        public static class ResourceFolders
        {
            public const string ConfirmAccount = "EmailTemplates.ConfirmAccount";
        }
    }
}
