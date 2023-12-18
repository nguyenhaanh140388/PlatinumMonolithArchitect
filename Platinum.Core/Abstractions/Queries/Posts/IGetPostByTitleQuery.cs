
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anhny010920.Core.Abstractions.Queries.Posts
{
    public interface IGetPostByTitleQuery<T> : 
        IQueryHandler<IEnumerable<Post>>, IQueryHandlerAsync<IEnumerable<Post>>
    {
        string Title { get; set; }
    }
}
