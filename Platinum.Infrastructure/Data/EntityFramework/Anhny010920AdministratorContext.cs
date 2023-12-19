// <copyright file="Anhny010920AdministratorContext.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;

namespace Platinum.Infrastructure.Data.EntityFramework
{
    public interface IAnhny010920AdministratorContext
    {
        DbSet<EmailTemplate> EmailTemplates { get; set; }
    }

    /// <summary>
    /// Anhny010920AdministratorContext.
    /// </summary>
    /// <seealso cref="IdentityDbContext" />
    /// <seealso cref="DbContext" />
    public partial class Anhny010920AdministratorContext : IdentityDbContext<ApplicationUser,
        ApplicationRole,
        Guid,
        ApplicationUserClaim,
        ApplicationUserRole,
        ApplicationUserLogin,
        ApplicationRoleClaim,
        ApplicationUserToken>, IAnhny010920AdministratorContext
    {
        #region identity
        //public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        //public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        //public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        //public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        //public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        // public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        //public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        #endregion

        public virtual DbSet<EmailTemplate> EmailTemplates { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Anhny010920AdministratorContext" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public Anhny010920AdministratorContext(DbContextOptions<Anhny010920AdministratorContext> options)
: base(options)
        {
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    ////base.OnModelCreating(modelBuilder);
        //    //modelBuilder.Entity<AspNetUsers>(b =>
        //    //{
        //    //    // Primary key
        //    //    //b.HasKey(u => u.Id);
        //    //    b.Property(e => e.Id)
        //    //     .HasColumnName("Id")
        //    //     .ValueGeneratedNever();

        //    //    // Indexes for "normalized" username and email, to allow efficient lookups
        //    //    b.HasIndex(u => u.NormalizedUserName).HasName("UserNameIndex").IsUnique();
        //    //    b.HasIndex(u => u.NormalizedEmail).HasName("EmailIndex");

        //    //    // Maps to the AspNetUsers table
        //    //    b.ToTable("AspNetUsers");

        //    //    // A concurrency token for use with the optimistic concurrency checking
        //    //    b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

        //    //    // Limit the size of columns to use efficient database types
        //    //    b.Property(u => u.UserName).HasMaxLength(256);
        //    //    b.Property(u => u.NormalizedUserName).HasMaxLength(256);
        //    //    b.Property(u => u.Email).HasMaxLength(256);
        //    //    b.Property(u => u.NormalizedEmail).HasMaxLength(256);

        //    //    // The relationships between User and other entity types
        //    //    // Note that these relationships are configured with no navigation properties

        //    //    // Each User can have many UserClaims
        //    //    b.HasMany<AspNetUserClaims>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();

        //    //    // Each User can have many UserLogins
        //    //    b.HasMany<AspNetUserLogins>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();

        //    //    // Each User can have many UserTokens
        //    //    b.HasMany<AspNetUserTokens>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();

        //    //    // Each User can have many entries in the UserRole join table
        //    //    b.HasMany<AspNetUserRoles>().WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
        //    //});

        //    //modelBuilder.Entity<AspNetUserClaims>(b =>
        //    //{
        //    //    // Primary key
        //    //    b.HasKey(uc => uc.Id);

        //    //    // Maps to the AspNetUserClaims table
        //    //    b.ToTable("AspNetUserClaims");
        //    //});

        //    //modelBuilder.Entity<AspNetUserLogins>(b =>
        //    //{
        //    //    // Composite primary key consisting of the LoginProvider and the key to use
        //    //    // with that provider
        //    //    b.HasKey(l => new { l.LoginProvider, l.ProviderKey });

        //    //    // Limit the size of the composite key columns due to common DB restrictions
        //    //    b.Property(l => l.LoginProvider).HasMaxLength(128);
        //    //    b.Property(l => l.ProviderKey).HasMaxLength(128);

        //    //    // Maps to the AspNetUserLogins table
        //    //    b.ToTable("AspNetUserLogins");
        //    //});

        //    //modelBuilder.Entity<AspNetUserTokens>(b =>
        //    //{
        //    //    // Composite primary key consisting of the UserId, LoginProvider and Name
        //    //    b.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

        //    //    // Limit the size of the composite key columns due to common DB restrictions
        //    //    b.Property(t => t.LoginProvider).HasMaxLength(256);
        //    //    b.Property(t => t.Name).HasMaxLength(256);

        //    //    // Maps to the AspNetUserTokens table
        //    //    b.ToTable("AspNetUserTokens");
        //    //});

        //    //modelBuilder.Entity<AspNetRoles>(b =>
        //    //{
        //    //    // Primary key
        //    //    b.HasKey(r => r.Id);

        //    //    // Index for "normalized" role name to allow efficient lookups
        //    //    b.HasIndex(r => r.NormalizedName).HasName("RoleNameIndex").IsUnique();

        //    //    // Maps to the AspNetRoles table
        //    //    b.ToTable("AspNetRoles");

        //    //    // A concurrency token for use with the optimistic concurrency checking
        //    //    b.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();

        //    //    // Limit the size of columns to use efficient database types
        //    //    b.Property(u => u.Name).HasMaxLength(256);
        //    //    b.Property(u => u.NormalizedName).HasMaxLength(256);

        //    //    // The relationships between Role and other entity types
        //    //    // Note that these relationships are configured with no navigation properties

        //    //    // Each Role can have many entries in the UserRole join table
        //    //    b.HasMany<AspNetUserRoles>().WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();

        //    //    // Each Role can have many associated RoleClaims
        //    //    b.HasMany<AspNetRoleClaims>().WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();
        //    //});

        //    //modelBuilder.Entity<AspNetRoleClaims>(b =>
        //    //{
        //    //    // Primary key
        //    //    b.HasKey(rc => rc.Id);

        //    //    // Maps to the AspNetRoleClaims table
        //    //    b.ToTable("AspNetRoleClaims");
        //    //});

        //    //modelBuilder.Entity<AspNetUserRoles>(b =>
        //    //{
        //    //    // Primary key
        //    //    b.HasKey(r => new { r.UserId, r.RoleId });

        //    //    // Maps to the AspNetUserRoles table
        //    //    b.ToTable("AspNetUserRoles");
        //    //});
        //}
    }
}
