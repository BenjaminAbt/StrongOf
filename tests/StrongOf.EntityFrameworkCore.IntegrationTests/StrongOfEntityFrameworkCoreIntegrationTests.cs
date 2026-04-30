// Copyright (c) BEN ABT (https://benjamin-abt.com) - all rights reserved

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using StrongOf.EntityFrameworkCore;
using Xunit;

namespace StrongOf.EntityFrameworkCore.IntegrationTests;

public sealed class StrongOfEntityFrameworkCoreIntegrationTests
{
    [Fact]
    public async Task ConfigureConventions_RegisterStrongOf_RoundTripsStrongTypes()
    {
        DbContextOptions<ConventionDbContext> options = CreateOptions<ConventionDbContext>();
        EfUserId userId = new(Guid.NewGuid());
        EfTenantId tenantId = new("tenant-a");

        await using (ConventionDbContext dbContext = new(options))
        {
            dbContext.Entities.Add(new EfEntity
            {
                Id = userId,
                TenantId = tenantId,
                Version = new EfVersion(42),
            });

            await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        await using (ConventionDbContext dbContext = new(options))
        {
            EfEntity? loaded = await dbContext.Entities.SingleOrDefaultAsync(
                e => e.Id == userId,
                TestContext.Current.CancellationToken);

            Assert.NotNull(loaded);
            Assert.Equal(userId.Value, loaded.Id.Value);
            Assert.Equal(tenantId.Value, loaded.TenantId.Value);
            Assert.Equal(42L, loaded.Version.Value);
        }
    }

    [Fact]
    public async Task ConfigureConventions_RegisterStrongOfFromAssembly_ConfiguresConverters()
    {
        DbContextOptions<AssemblyConventionDbContext> options = CreateOptions<AssemblyConventionDbContext>();

        await using AssemblyConventionDbContext dbContext = new(options);

        IProperty idProperty = dbContext.Model.FindEntityType(typeof(EfEntity))!.FindProperty(nameof(EfEntity.Id))!;
        IProperty tenantProperty = dbContext.Model.FindEntityType(typeof(EfEntity))!.FindProperty(nameof(EfEntity.TenantId))!;

        Assert.NotNull(idProperty.GetValueConverter());
        Assert.NotNull(tenantProperty.GetValueConverter());
    }

    [Fact]
    public async Task OnModelCreating_HasStrongOfConversion_RoundTripsStrongTypes()
    {
        DbContextOptions<PropertyBuilderDbContext> options = CreateOptions<PropertyBuilderDbContext>();
        EfUserId userId = new(Guid.NewGuid());

        await using (PropertyBuilderDbContext dbContext = new(options))
        {
            dbContext.Entities.Add(new EfEntity
            {
                Id = userId,
                TenantId = new EfTenantId("tenant-b"),
                Version = new EfVersion(7),
            });

            await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        await using (PropertyBuilderDbContext dbContext = new(options))
        {
            EfEntity? loaded = await dbContext.Entities.SingleOrDefaultAsync(
                e => e.TenantId == new EfTenantId("tenant-b"),
                TestContext.Current.CancellationToken);

            Assert.NotNull(loaded);
            Assert.Equal(userId.Value, loaded.Id.Value);
            Assert.Equal(7L, loaded.Version.Value);
        }
    }

    private static DbContextOptions<TContext> CreateOptions<TContext>()
        where TContext : DbContext
    {
        return new DbContextOptionsBuilder<TContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString("N"))
            .Options;
    }

    public sealed class EfUserId(Guid value) : StrongGuid<EfUserId>(value) { }

    public sealed class EfTenantId(string value) : StrongString<EfTenantId>(value) { }

    public sealed class EfVersion(long value) : StrongInt64<EfVersion>(value) { }

    public sealed class EfEntity
    {
        public EfUserId Id { get; set; } = null!;

        public EfTenantId TenantId { get; set; } = null!;

        public EfVersion Version { get; set; } = null!;
    }

    private sealed class ConventionDbContext(DbContextOptions<ConventionDbContext> options)
        : DbContext(options)
    {
        public DbSet<EfEntity> Entities => Set<EfEntity>();

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.RegisterStrongOf<EfUserId, Guid>();
            configurationBuilder.RegisterStrongOf<EfTenantId, string>();
            configurationBuilder.RegisterStrongOf<EfVersion, long>();
        }
    }

    private sealed class AssemblyConventionDbContext(DbContextOptions<AssemblyConventionDbContext> options)
        : DbContext(options)
    {
        public DbSet<EfEntity> Entities => Set<EfEntity>();

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.RegisterStrongOfFromAssembly(typeof(EfUserId).Assembly);
        }
    }

    private sealed class PropertyBuilderDbContext(DbContextOptions<PropertyBuilderDbContext> options)
        : DbContext(options)
    {
        public DbSet<EfEntity> Entities => Set<EfEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EfEntity>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasStrongOfConversion<EfUserId, Guid>();

                entity.Property(e => e.TenantId)
                    .HasStrongOfConversion<EfTenantId, string>();

                entity.Property(e => e.Version)
                    .HasStrongOfConversion<EfVersion, long>();
            });
        }
    }
}
