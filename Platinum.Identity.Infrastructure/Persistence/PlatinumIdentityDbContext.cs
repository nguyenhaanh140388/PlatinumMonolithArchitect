// <copyright file="Anhny010920AdministratorContext.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Platinum.Identity.Core.Entities;

namespace Platinum.Identity.Infrastructure.Persistence
{
    public interface IPlatinumIdentityDbContext
    {
       
    }

    /// <summary>
    /// Anhny010920AdministratorContext.
    /// </summary>
    /// <seealso cref="IdentityDbContext" />
    /// <seealso cref="DbContext" />
    public partial class PlatinumIdentityDbContext : IdentityDbContext<ApplicationUser,
        ApplicationRole,
        Guid,
        ApplicationUserClaim,
        ApplicationUserRole,
        ApplicationUserLogin,
        ApplicationRoleClaim,
        ApplicationUserToken>, IPlatinumIdentityDbContext
    {
        #region identity
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="PlatinumIdentityDbContext" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public PlatinumIdentityDbContext(DbContextOptions<PlatinumIdentityDbContext> options)
            : base(options)
        {
        }
    }
}
