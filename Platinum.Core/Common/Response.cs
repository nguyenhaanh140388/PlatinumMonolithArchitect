namespace Platinum.Core.Common
{
    public class Response
    {
        public Response()
        {
        }

        public Response(List<string> errors = null, string message = null)
        {
            Message = message;
            Errors = errors;
        }

        public Response(bool succeeded = true, string message = null)
        {
            Succeeded = succeeded;
            Message = message;
        }
        public Response(bool succeeded = true)
        {
            Succeeded = succeeded;
        }

        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
    }
}
