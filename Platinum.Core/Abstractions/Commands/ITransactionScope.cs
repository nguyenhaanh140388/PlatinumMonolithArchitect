// <copyright file="ITransactionScope.cs" company="Platinum">
// Copyright (c) Platinum. All rights reserved.
// </copyright>

namespace Platinum.Core.Abstractions.Commands
{
    public interface ITransactionScope
    {
        void Commit();

        void Rollback();
    }
}
