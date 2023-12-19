namespace Platinum.Core.Enums
{
    public enum BulkTypes
    {
        /// <summary>
        /// Bulk Insert.
        /// </summary>
        Insert,
        /// <summary>
        /// Bulk Update.
        /// </summary>
        Update,
        /// <summary>
        /// Bulk Delete.
        /// </summary>
        Delete,
        /// <summary>
        /// Bulk SoftDelete.
        /// </summary>
        SoftDelete,
        /// <summary>
        /// Bulk Merge.
        /// </summary>
        Merge,
        Synchronize,
        SaveChanges,
    }
}
