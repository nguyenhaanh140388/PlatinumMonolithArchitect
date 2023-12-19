using Platinum.Core.Abstractions.Models.Response;

namespace Platinum.Core.Common
{
    public class ResponsePaginated<T> : ResponseList<T>, IPaginatedViewModel<T>
       where T : IViewModel
    {
        public ResponsePaginated()
        {
        }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPageCount { get; set; }

        public bool HasNextPage { get; set; }

        public bool HasPreviousPage { get; set; }

    }
}
