using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Platinum.Catalog.Core.Abstractions.EntityFramework;
using Platinum.Catalog.Infrastructure.Configurations;
using Platinum.Core.Common;
using Platinum.Core.Entities;

namespace Platinum.Catalog.Core.Entities;

public partial class PlatinumCatalogContext : BaseDBContext<PlatinumCatalogContext>, IPlatinumCatalogContext
{
    public PlatinumCatalogContext()
    {
    }

    public PlatinumCatalogContext(DbContextOptions<PlatinumCatalogContext> options)
        : base(options)
    {

    }

    #region tracking entity
    public DbSet<Audit> AuditLogs { get; set; }
    #endregion

    #region entites
    public virtual DbSet<AddressBook> AddressBooks { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartAttribute> CartAttributes { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Configuration> Configurations { get; set; }

    public virtual DbSet<ConfigurationGroup> ConfigurationGroups { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Currency> Currencies { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerInfo> CustomerInfos { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<ManufacturersInfo> ManufacturersInfos { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderProductAttribute> OrderProductAttributes { get; set; }

    public virtual DbSet<OrdersProduct> OrdersProducts { get; set; }

    public virtual DbSet<OrdersStatus> OrdersStatuses { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductsAttribute> ProductsAttributes { get; set; }

    public virtual DbSet<ProductsOption> ProductsOptions { get; set; }

    public virtual DbSet<ProductsOptionsValue> ProductsOptionsValues { get; set; }

    public virtual DbSet<ProductsOptionsValuesMapping> ProductsOptionsValuesMappings { get; set; }

    public virtual DbSet<Productsdetail> Productsdetails { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<ReviewsDetail> ReviewsDetails { get; set; }

    public virtual DbSet<WhoIsOnline> WhoIsOnlines { get; set; }
    public DatabaseFacade DatabaseFacade => this.Database;
    #endregion

    private async Task OnBeforeSaveChanges()
    {
        TrackingChanges(Guid.Empty);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await OnBeforeSaveChanges();
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // optionsBuilder.UseLazyLoadingProxies(); // Notice this change  
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        PlatinumCatalogConfig.ConfigureEf(modelBuilder);
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
