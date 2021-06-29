using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace EShopOnAbp.AdministrationService.EntityFrameworkCore
{
    /* This is your actual DbContext used on runtime.
     * It includes only your entities.
     * It does not include entities of the used modules, because each module has already
     * its own DbContext class. If you want to share some database tables with the used modules,
     * just create a structure like done for AppUser.
     *
     * Don't use this DbContext for database migrations since it does not contain tables of the
     * used modules (as explained above). See AdministrationServiceMigrationsDbContext for migrations.
     */
    [ConnectionStringName(AdministrationServiceDbProperties.ConnectionStringName)]
    public class AdministrationServiceDbContext 
        : AbpDbContext<AdministrationServiceDbContext>,
        IPermissionManagementDbContext,
        ISettingManagementDbContext,
        IFeatureManagementDbContext,
        IAuditLoggingDbContext,
        IBlobStoringDbContext
    {
        public AdministrationServiceDbContext(DbContextOptions<AdministrationServiceDbContext> options)
            : base(options)
        {

        }

        public DbSet<PermissionGrant> PermissionGrants { get; }
        public DbSet<Setting> Settings { get; }
        public DbSet<FeatureValue> FeatureValues { get; }
        public DbSet<AuditLog> AuditLogs { get; }
        public DbSet<DatabaseBlobContainer> BlobContainers { get; }
        public DbSet<DatabaseBlob> Blobs { get; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigurePermissionManagement();
            builder.ConfigureSettingManagement();
            builder.ConfigureFeatureManagement();
            builder.ConfigureAuditLogging();
            builder.ConfigureBlobStoring();
        }
    }
}
