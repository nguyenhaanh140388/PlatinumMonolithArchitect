using Platinum.Core.Abstractions.Dtos;

namespace Platinum.Core.Dtos
{
    public class PaginatedDto<TDto> : IPaginatedDto<TDto>
        where TDto : IDto
    {
        public PaginatedDto()
        {
        }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPageCount { get; set; }

        public bool HasNextPage { get; set; }

        public bool HasPreviousPage { get; set; }

        public IEnumerable<TDto> Items { get; set; }
    }
}
