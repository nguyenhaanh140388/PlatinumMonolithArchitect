// <copyright file="ITransactionScope.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

namespace Anhny010920.Core.Abstractions.Commands
{
    public interface ITransactionScope
    {
        void Commit();

        void Rollback();
    }
}
