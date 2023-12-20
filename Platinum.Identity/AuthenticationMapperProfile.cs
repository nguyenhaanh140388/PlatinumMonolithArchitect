// <copyright file="AuthenticationMapperProfile.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

using Platinum.Core.Common;
using Platinum.Identity.Core.Entities;
using Platinum.Identity.Core.Models;

namespace Platinum.Identity
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
            CreateMap<AuthenticateRequest, ApplicationUser>();
        }
    }
}
