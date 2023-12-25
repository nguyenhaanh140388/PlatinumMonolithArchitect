// <copyright file="IBaseRepository.cs" company="Platinum">
// Copyright (c) Platinum. All rights reserved.
// </copyright>

using Platinum.Core.Abstractions.Repositories;
using Platinum.Identity.Core.Templates;

namespace Platinum.Identity.Core.Abstractions.Repositories
{
    public interface IEmailTemplateRepository : IBaseRepository<EmailTemplate>
    {
    }
}
