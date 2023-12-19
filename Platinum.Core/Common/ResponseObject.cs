namespace Platinum.Core.Common
{
    public class ResponseObject<T> : Response
    {
        public ResponseObject()
        {
        }

        public ResponseObject(T data = default, List<string> errors = null, string message = null)
        {
            Message = message;
            Data = data;
            Errors = errors;
        }

        public ResponseObject(T data, bool succeeded, string message = null)
        {
            Succeeded = succeeded;
            Message = message;
            Data = data;
        }

        public ResponseObject(T data = default, bool succeeded = true)
        {
            Succeeded = succeeded;
            Data = data;
        }

        public ResponseObject(bool succeeded = true)
        {
            Succeeded = succeeded;
        }

        public T Data { get; set; }
    }
}
