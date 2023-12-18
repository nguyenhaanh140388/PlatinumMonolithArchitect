using Anhny010920.Core.Domain.Common;
using Anhny010920.Core.Wrappers;
using Platinum.Core.Abstractions.Models.Response;
using System.Collections.Generic;

namespace Platinum.Core.Extensions
{
    /// <summary>
    /// PaginatedListExtensions.
    /// </summary>
    public static class PaginatedListExtensions
    {
        //public static ResponsePaginated<T> ToPagedResponse<T>(
        //    this PaginatedList<T> source)
        //    where T : IViewModel
        //{
        //    return new ResponsePaginated<T>
        //    {
        //        PageIndex = source.PageIndex,
        //        PageSize = source.PageSize,
        //        TotalCount = source.TotalCount,
        //        TotalPageCount = source.TotalPageCount,
        //        HasNextPage = source.HasNextPage,
        //        HasPreviousPage = source.HasPreviousPage,
        //        Data = source,
        //    };
        //}

        public static ResponsePaginated<T> ToPagedResponse<T>(
            this PaginatedList<T> source, string message, List<string> errors, bool succeeded)
            where T : IViewModel
        {
            return new ResponsePaginated<T>
            {
                PageIndex = source.PageIndex,
                PageSize = source.PageSize,
                TotalCount = source.TotalCount,
                TotalPageCount = source.TotalPageCount,
                HasNextPage = source.HasNextPage,
                HasPreviousPage = source.HasPreviousPage,
                Message = message,
                Errors = errors,
                Succeeded = succeeded,
                DataSource = source,
            };
        }

        public static ResponsePaginated<T> ToPagedResponse<T>(
            this PaginatedList<T> source)
            where T : IViewModel
        {
            return ToPagedResponse(source, "", null, true);
        }
    }
}
