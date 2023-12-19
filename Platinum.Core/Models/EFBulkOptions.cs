using Platinum.Core.Common;
using System.Linq.Expressions;

namespace Platinum.Core.Models
{
    public class EFBulkOptions<TEntity> where TEntity : EntityBase, new()
    {
        public BulkInsertOptions<TEntity> BulkInsertOptions { get; set; }
        public BulkUpdateOptions<TEntity> BulkUpdateOptions { get; set; }
        public BulkMergeOptions<TEntity> BulkMergeOptions { get; set; }
        public BulkSynchronizeOptions<TEntity> BulkSynchronizeOptions { get; set; }
        public bool IncludeGraph { get; set; }
        public Expression<Func<TEntity, object>> ColumnInputExpression { get; set; }
        public Expression<Func<TEntity, object>> ColumnPrimaryKeyExpression { get; set; }
    }

    public class BulkInsertOptions<TEntity>
    {
        // Insert only if the entity does not already exists
        public bool InsertIfNotExists { get; set; }
        public bool InsertKeepIdentity { get; set; }
        public bool AutoMapOutputDirection { get; set; }
        public Expression<Func<TEntity, object>> IgnoreOnInsertExpression { get; set; }
        public BulkInsertOptions()
        {
        }
    }

    public class BulkUpdateOptions<TEntity>
    {
        public Expression<Func<TEntity, object>> ColumnIgnoreExpression { get; set; }
        public Expression<Func<TEntity, object>> IgnoreOnMergeInsertExpression { get; set; }
        public Expression<Func<TEntity, object>> IgnoreOnMergeUpdateExpression { get; set; }
        public BulkUpdateOptions()
        {

        }
    }

    public class BulkSynchronizeOptions<TEntity>
    {
        public bool SynchronizeKeepIdentity { get; set; }
        public Expression<Func<TEntity, object>> ColumnIgnoreExpression { get; set; }
        public Expression<Func<TEntity, object>> IgnoreOnSynchronizeInsertExpression { get; set; }
        public Expression<Func<TEntity, object>> IgnoreOnSynchronizeUpdateExpression { get; set; }
        public BulkSynchronizeOptions()
        {

        }

    }
    public class BulkMergeOptions<TEntity>
    {
        public bool MergeKeepIdentity { get; set; }
        public Expression<Func<TEntity, object>> IgnoreOnUpdateExpression { get; set; }

        public BulkMergeOptions()
        {

        }
    }
}
