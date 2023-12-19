namespace Platinum.Core.Abstractions.Models.Response
{
    public interface IPaginatedViewModel<out T>
       where T : IViewModel
    {
        int PageIndex { get; set; }

        int PageSize { get; set; }

        int TotalCount { get; set; }

        int TotalPageCount { get; set; }

        bool HasNextPage { get; set; }

        bool HasPreviousPage { get; set; }
    }
}
