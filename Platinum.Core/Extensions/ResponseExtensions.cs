using Platinum.Core.Common;

namespace Platinum.Core.Extensions
{
    public static class ResponseExtensions
    {
        public static ResponseList<T> ToResponseList<T>(
            this IEnumerable<T> source, List<string> errors = null, string message = null)
            where T : class
        {
            return new ResponseList<T>(source, errors, message);
        }

        public static ResponseList<T> ToResponseList<T>(
            this IEnumerable<T> source, bool succeeded = true)
            where T : class
        {
            return new ResponseList<T>(source, succeeded);
        }

        public static ResponseList<T> ToResponseList<T>(
           this IEnumerable<T> source, bool succeeded = true, string message = null)
           where T : class
        {
            return new ResponseList<T>(source, succeeded, message);
        }

        public static ResponseObject<T> ToResponseObject<T>(
            this T source, List<string> errors = null, string message = null)
            where T : class
        {
            return new ResponseObject<T>(source, errors, message);
        }

        public static ResponseObject<T> ToResponseObject<T>(
            this T source, bool succeeded = true)
            where T : class
        {
            return new ResponseObject<T>(source, succeeded);
        }

        public static ResponseObject<T> ToResponseObject<T>(
           this T source, bool succeeded = true, string message = null)
           where T : class
        {
            return new ResponseObject<T>(source, succeeded, message);
        }
    }
}
