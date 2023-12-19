// <copyright file="AuthenticationMapperProfile.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

using Platinum.Core.Common;
using Platinum.Core.Models;
using Platinum.Identity.Core.Entities;

namespace Platinum.Infrastructure.Data.Mapping
{
    /// <summary>
    /// AuthenticationMapperProfile.
    /// </summary>
    /// <seealso cref="MapperProfileBase" />
    public class AuthenticationMapperProfile : MapperProfileBase
    {
        /// <summary>
        /// Creates the mappings.
        /// </summary>
        protected override void CreateMappings()
        {
            base.CreateMappings();
            this.CreateMap<AuthenticateRequest, ApplicationUser>();
        }
    }
}
