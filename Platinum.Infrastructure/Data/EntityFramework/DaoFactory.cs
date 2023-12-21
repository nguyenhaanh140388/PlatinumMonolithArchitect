// <copyright file="DaoFactory.cs" company="Platinum">
// Copyright (c) Platinum. All rights reserved.
// </copyright>

using Microsoft.Extensions.Configuration;
using Platinum.Core.Abstractions.Dao;
using Platinum.Core.Abstractions.Dapper;
using Platinum.Core.Abstractions.Queries;
using Platinum.Core.Enums;
using Platinum.Core.Utils;
using Platinum.Infrastructure.Dao.Dappers;

namespace Platinum.Infrastructure.Data.EntityFramework
{
    /// <summary>
    /// DaoFactory.
    /// </summary>
    /// <seealso cref="Infrastructure.Abstractions.Dao.IDaoFactory" />
    /// <seealso cref="IDaoFactory" />
    internal sealed class DaoFactory : IDaoFactory
    {
        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public IConfiguration Configuration => (IConfiguration)ApplicationHttpContext.GetService(typeof(IConfiguration));

        /// <summary>
        /// Gets the dapper DAO.
        /// </summary>
        /// <param name="dataContextNames">The data context names.</param>
        /// <returns>IDapper.</returns>
        IDapper IDaoFactory.GetDapperDao(DataContextNames dataContextNames)
        {
            string connectString;
            switch (dataContextNames)
            {
                case DataContextNames.ProductSystem:
                    connectString = Configuration.GetConnectionString("PlatinumCatalog");
                    break;
                case DataContextNames.BlogSystem:
                    connectString = Configuration.GetConnectionString("PlatinumBlog");
                    break;
                case DataContextNames.AdministratorSystem:
                    connectString = Configuration.GetConnectionString("PlatinumAdministrator");
                    break;
                default:
                    connectString = Configuration.GetConnectionString("PlatinumCatalog"); break;
            }

            return new DapperQuery(connectString);
        }

        /// <summary>
        /// Gets the entity framework DAO.
        /// </summary>
        /// <param name="dataContextNames">The data context names.</param>
        /// <returns>
        /// IDaoFactory.
        /// </returns>
        /// <exception cref="KeyNotFoundException">KeyNotFoundException.</exception>
        IEntityQuery IDaoFactory.GetEntityFrameworkDao(DataContextNames dataContextNames)
        {
            //switch (dataContextNames)
            //{
            //case DataContextNames.ProductSystem:
            //    return new PlatinumDao((PlatinumProductContext)ApplicationContext.GetService(typeof(PlatinumProductContext)));
            //case DataContextNames.BlogSystem:
            //    return new PlatinumDao((PlatinumlogContext)ApplicationContext.GetService(typeof(PlatinumBlogContext)));
            //case DataContextNames.AdministratorSystem:
            //    return new PlatinumDao((PlatinumAdministratorContext)ApplicationContext.GetService(typeof(PlatinumAdministratorContext)));
            //default:
            throw new KeyNotFoundException();
            //}
        }
    }
}
