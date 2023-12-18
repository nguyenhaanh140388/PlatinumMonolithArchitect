﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anhny010920.Core.Abstractions.Queries.Posts
{
    public interface IGetPostByIdQuery<T> : 
        IQueryHandler<Post>, IQueryHandlerAsync<Post>
    {
        int? Id { get; set; }
    }
}
