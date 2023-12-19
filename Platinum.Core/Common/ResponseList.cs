namespace Platinum.Core.Common
{
    public class ResponseList<T> : Response
    {
        public ResponseList()
        {
        }

        public ResponseList(IEnumerable<T> data = default, List<string> errors = null, string message = null)
        {
            Message = message;
            DataSource = data;
            Errors = errors;
        }

        public ResponseList(IEnumerable<T> data = default, bool succeeded = true, string message = null)
        {
            Succeeded = succeeded;
            Message = message;
            DataSource = data;
        }

        public ResponseList(IEnumerable<T> data = default, bool succeeded = true)
        {
            Succeeded = succeeded;
            DataSource = data;
        }

        public ResponseList(bool succeeded = true)
        {
            Succeeded = succeeded;
        }

        public IEnumerable<T> DataSource { get; set; }
    }
}
