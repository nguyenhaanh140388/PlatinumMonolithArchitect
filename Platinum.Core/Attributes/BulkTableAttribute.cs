namespace Platinum.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class BulkTableAttribute : Attribute
    {
        public string Name { get; private set; }
        public bool IsTempTable { get; private set; }
        public string InsertColumns { get; private set; }
        public string UpdateColumns { get; private set; }
        public string OutputColumns { get; private set; }

        public BulkTableAttribute(string name
            , bool isTempTable = false
            , string insertColumns = null
            , string updateColumns = null
            , string outputColumns = null)
        {
            Name = name;
            IsTempTable = isTempTable;
            InsertColumns = insertColumns;
            UpdateColumns = insertColumns;
            OutputColumns = outputColumns;
        }
    }
}
