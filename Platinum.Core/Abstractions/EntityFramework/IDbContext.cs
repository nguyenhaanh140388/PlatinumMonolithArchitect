using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Platinum.Core.Abstractions.EntityFramework
{
    public interface IDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        DatabaseFacade DatabaseFacade { get; set; }
        void Dispose();
    }
}
