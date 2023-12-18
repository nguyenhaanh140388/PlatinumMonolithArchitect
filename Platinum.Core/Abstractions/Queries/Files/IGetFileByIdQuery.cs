
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anhny010920.Core.Abstractions.Queries.Files
{
    public interface IGetFileByIdQuery<T> : 
        IQueryHandler<File>, IQueryHandlerAsync<File>
    {
        Guid? Id { get; set; }
    }
}
