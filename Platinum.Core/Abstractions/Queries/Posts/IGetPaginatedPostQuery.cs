﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platinum.Core.Abstractions.Queries.Posts
{
    public interface IGetPaginatedPostQuery<in T> :
        IQueryHandler<IEnumerable<Post>>, IQueryHandlerAsync<IEnumerable<Post>>
    {
        int PageNumber { get; set; }
        int PageCount { get; set; }
    }
}
