
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anhny010920.Core.Abstractions.Queries.Comments
{
    interface IGetCommentsByPostQuery<in T> :
        IQueryHandler<IEnumerable<Comment>>, IQueryHandlerAsync<IEnumerable<Comment>>
    {
        int PostId { get; set; }
    }
}
