﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platinum.Core.Abstractions.Queries.Posts
{
    public interface IGetPostByAuthorQuery<T> :
        IQueryHandler<IEnumerable<Post>>, IQueryHandlerAsync<IEnumerable<Post>>
    {
        string Author { get; set; }
    }
}
