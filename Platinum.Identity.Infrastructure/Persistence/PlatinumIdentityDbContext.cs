// <copyright file="PlatinumIdentityDbContext.cs" company="Platinum">
// Copyright (c) Platinum. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Platinum.Identity.Core.Entities;
using Platinum.Identity.Core.Templates;

namespace Platinum.Identity.Infrastructure.Persistence
{
    public interface IPlatinumIdentityDbContext
    {
        DbSet<EmailTemplate> EmailTemplates { get; set; }
    }

    /// <summary>
    /// PlatinumIdentityDbContext.
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
        #region settings
        public virtual DbSet<EmailTemplate> EmailTemplates { get; set; }
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
