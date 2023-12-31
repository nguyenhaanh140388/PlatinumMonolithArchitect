﻿namespace Platinum.Core.Common
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPageCount { get; private set; }

        public PaginatedList(
            int pageIndex, int pageSize,
            int totalCount, List<T> source)
        {

            AddRange(source);

            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPageCount =
                (int)Math.Ceiling(totalCount / (double)pageSize);
        }

        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPageCount;
    }
}
