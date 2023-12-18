﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platinum.Core.Abstractions.Queries.People
{
    public interface IGetPersonByIdQuery<T> :
        IQueryHandler<Person>, IQueryHandlerAsync<Person>
    {
        int? Id { get; set; }
    }
}
