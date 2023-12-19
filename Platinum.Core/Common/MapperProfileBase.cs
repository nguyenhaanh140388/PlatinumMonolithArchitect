// <copyright file="MapperProfileBase.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

using AutoMapper;

namespace Platinum.Core.Common
{
    /// <summary>
    /// MapperProfileBase.
    /// </summary>
    /// <seealso cref="Profile" />
    public class MapperProfileBase : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapperProfileBase" /> class.
        /// </summary>
        public MapperProfileBase()
        {
            // Naming Conventions.
            this.SourceMemberNamingConvention = new LowerUnderscoreNamingConvention();
            this.DestinationMemberNamingConvention = new PascalCaseNamingConvention();

            // Replace character.
            this.ReplaceMemberName("Special char", "replaced char");

            // Recognizing pre/postfixes
            this.RecognizePrefixes("frm");
            this.RecognizePostfixes("_");

            // don't map any fields
            // ShouldMapField = fi => false;

            // map properties with a public or private getter
            // ShouldMapProperty = pi =>
            // pi.GetMethod != null && (pi.GetMethod.IsPublic || pi.GetMethod.IsPrivate);
            // map properties with public or internal getters
            // ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
            CreateMappings();
        }

        /// <summary>
        /// Creates the mappings.
        /// </summary>
        protected virtual void CreateMappings()
        {
        }
    }
}
