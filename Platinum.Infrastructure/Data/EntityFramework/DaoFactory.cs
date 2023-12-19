// <copyright file="DaoFactory.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

using Microsoft.Extensions.Configuration;

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
        public IConfiguration Configuration => (IConfiguration)ApplicationContext.GetService(typeof(IConfiguration));

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
                    connectString = Configuration.GetConnectionString("Anhny010920Catalog");
                    break;
                case DataContextNames.BlogSystem:
                    connectString = Configuration.GetConnectionString("Anhny010920Blog");
                    break;
                case DataContextNames.AdministratorSystem:
                    connectString = Configuration.GetConnectionString("Anhny010920Administrator");
                    break;
                default:
                    connectString = Configuration.GetConnectionString("Anhny010920Catalog"); break;
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
            //    return new Anhny010290Dao((Anhny010920ProductContext)ApplicationContext.GetService(typeof(Anhny010920ProductContext)));
            //case DataContextNames.BlogSystem:
            //    return new Anhny010290Dao((Anhny010920BlogContext)ApplicationContext.GetService(typeof(Anhny010920BlogContext)));
            //case DataContextNames.AdministratorSystem:
            //    return new Anhny010290Dao((Anhny010920AdministratorContext)ApplicationContext.GetService(typeof(Anhny010920AdministratorContext)));
            //default:
            throw new KeyNotFoundException();
            //}
        }
    }
}
