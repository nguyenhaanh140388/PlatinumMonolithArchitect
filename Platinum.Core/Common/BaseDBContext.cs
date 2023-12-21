using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Platinum.Core.Attributes;
using Platinum.Core.Entities;
using Platinum.Core.Enums;

namespace Platinum.Core.Common
{
    public abstract class BaseDBContext<TDbContext> : DbContext where TDbContext : DbContext
    {
        protected static string connectionString = string.Empty;
        protected static string name = string.Empty;

        public BaseDBContext()
        {
        }

        public BaseDBContext(DbContextOptions<TDbContext> options) : base(options)
        {
            ChangeTracker.StateChanged += ChangeTracker_StateChanged!;
            ChangeTracker.Tracked += ChangeTracker_Tracked!;
            connectionString = GetConnectionString(name);
        }

        protected string GetConnectionString(string name)
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json");
            var config = builder.Build();
            var connectionString = config.GetConnectionString(name);

            return connectionString!;
        }

        public async Task TrackingChanges(Guid UserId)
        {
            ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();

            foreach (var entry in ChangeTracker.Entries())
            {
                var classAttribute = entry.Entity.GetType().GetCustomAttributes(typeof(TableTrackingAttribute), true);
                if (classAttribute.Length == 0)
                {
                    continue;
                }

                if (entry.Entity is Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;

                var auditEntry = new AuditEntry(entry)
                {
                    TableName = entry.Entity.GetType().Name,
                     UserId = UserId
                };

                var properties = entry.Properties.Where(x => x.Metadata.IsPrimaryKey() ||
                x.Metadata.PropertyInfo!.GetCustomAttributes(typeof(ColumnTrackingAttribute), true).Length > 0);

                foreach (var property in properties)
                {
                    string propertyName = property.Metadata.Name;

                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue!;
                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.AuditType = AuditType.Create;
                            auditEntry.NewValues[propertyName] = property.CurrentValue!;
                            break;
                        case EntityState.Deleted:
                            auditEntry.AuditType = AuditType.Delete;
                            auditEntry.OldValues[propertyName] = property.OriginalValue!;
                            break;
                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.ChangedColumns.Add(propertyName);
                                auditEntry.AuditType = AuditType.Update;
                                auditEntry.OldValues[propertyName] = property.OriginalValue!;
                                auditEntry.NewValues[propertyName] = property.CurrentValue!;
                            }
                            break;
                    }
                }

                auditEntries.Add(auditEntry);
            }

            //foreach (var auditEntry in auditEntries)
            //{
                await this.AddRangeAsync(auditEntries.Select(x=>x.ToAudit()));
            //}
        }

        public TDbContext GetPlatinumCatalogContext(string connnectString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return  (TDbContext)Activator.CreateInstance(typeof(TDbContext), optionsBuilder.Options)!;
        }

        public static TDbContext Instance
        {
            get
            {
                var optionsBuilder = new DbContextOptionsBuilder<TDbContext>();
                optionsBuilder.UseSqlServer(connectionString);
                return (TDbContext)Activator.CreateInstance(typeof(TDbContext), optionsBuilder.Options)!;
            }
        }

        private void ChangeTracker_Tracked(object sender, Microsoft.EntityFrameworkCore.ChangeTracking.EntityTrackedEventArgs e)
        {
            //if (!e.FromQuery && e.Entry.State == EntityState.Added && e.Entry.Entity is EntityBase entity)
            //    entity.CreatedDate = DateTime.Now;
        }

        private void ChangeTracker_StateChanged(object sender, Microsoft.EntityFrameworkCore.ChangeTracking.EntityStateChangedEventArgs e)
        {
            //if (e.NewState == EntityState.Modified && e.Entry.Entity is EntityBase entity)
            //    entity.ModifiedDate = DateTime.Now;
        }
    }
}
