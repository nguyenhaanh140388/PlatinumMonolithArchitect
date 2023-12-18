// <copyright file="TransactionScope.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Anhny010920.Core.Abstractions.Commands;
using Microsoft.EntityFrameworkCore.Storage;

namespace Platinum.Core.Commands
{
    /// <summary>
    /// DaoTransactionScope.
    /// </summary>
    /// <seealso cref="ITransactionScope" />
    /// <seealso cref="IDisposable" />
    public class DaoTransactionScope : ITransactionScope, IDisposable
    {
        // Flag: Has Dispose already been called?

        /// <summary>
        /// The disposed.
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="DaoTransactionScope"/> class.
        /// </summary>
        public DaoTransactionScope()
        {
            Transactions = new List<IDbContextTransaction>();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="DaoTransactionScope"/> class.
        /// </summary>
        ~DaoTransactionScope()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets or sets the transactions.
        /// </summary>
        /// <value>
        /// The transactions.
        /// </value>
        public List<IDbContextTransaction> Transactions { get; set; }

        /// <summary>
        /// Commits this instance.
        /// </summary>
        public void Commit()
        {
            Transactions.ForEach(item =>
            {
                item.Commit();
            });
        }

        /// <summary>
        /// Rollbacks this instance.
        /// </summary>
        public void Rollback()
        {
            Transactions.ForEach(item =>
            {
                item.Rollback();
            });
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                // Free any other managed objects here.
                //
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }
    }
}
