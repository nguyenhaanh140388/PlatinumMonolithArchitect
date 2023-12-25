// <copyright file="AuthenticationMapperProfile.cs" company="Platinum">
// Copyright (c) Platinum. All rights reserved.
// </copyright>

using Platinum.Catalog.Core.Entities;
using Platinum.Catalog.Core.Features.Categories.Commands;
using Platinum.Core.Common;

namespace Platinum.Infrastructure.Data.Mapping
{
    /// <summary>
    /// AuthenticationMapperProfile.
    /// </summary>
    /// <seealso cref="MapperProfileBase" />
    public class CatalogMapperProfile : MapperProfileBase
    {
        /// <summary>
        /// Creates the mappings.
        /// </summary>
        protected override void CreateMappings()
        {
            base.CreateMappings();
            this.CreateMap<CreateCategoryCommand, Category>();
        }
    }
}
