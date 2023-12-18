
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anhny010920.Core.Abstractions.Queries.Posts
{
    public interface IGetPostByUrlQuery<T> : 
        IQueryHandler<Post>, IQueryHandlerAsync<Post>
    {
        string Url { get; set; }
    }
}
