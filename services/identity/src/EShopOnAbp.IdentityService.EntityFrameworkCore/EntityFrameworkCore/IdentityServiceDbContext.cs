using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.ApiScopes;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.Devices;
using Volo.Abp.IdentityServer.EntityFrameworkCore;
using Volo.Abp.IdentityServer.Grants;
using Volo.Abp.IdentityServer.IdentityResources;

namespace EShopOnAbp.IdentityService.EntityFrameworkCore
{
    /* This is your actual DbContext used on runtime.
     * It includes only your entities.
     * It does not include entities of the used modules, because each module has already
     * its own DbContext class. If you want to share some database tables with the used modules,
     * just create a structure like done for AppUser.
     *
     * Don't use this DbContext for database migrations since it does not contain tables of the
     * used modules (as explained above). See IdentityServiceMigrationsDbContext for migrations.
     */
    [ConnectionStringName(IdentityServiceDbProperties.ConnectionStringName)]
    public class IdentityServiceDbContext : AbpDbContext<IdentityServiceDbContext>, IIdentityDbContext,
        IIdentityServerDbContext
    {
        public DbSet<IdentityUser> Users { get; }
        public DbSet<IdentityRole> Roles { get; }
        public DbSet<IdentityClaimType> ClaimTypes { get; }
        public DbSet<OrganizationUnit> OrganizationUnits { get; }
        public DbSet<IdentitySecurityLog> SecurityLogs { get; }
        public DbSet<IdentityLinkUser> LinkUsers { get; }
        public DbSet<ApiResource> ApiResources { get; }
        public DbSet<ApiResourceSecret> ApiResourceSecrets { get; }
        public DbSet<ApiResourceClaim> ApiResourceClaims { get; }
        public DbSet<ApiResourceScope> ApiResourceScopes { get; }
        public DbSet<ApiResourceProperty> ApiResourceProperties { get; }
        public DbSet<ApiScope> ApiScopes { get; }
        public DbSet<ApiScopeClaim> ApiScopeClaims { get; }
        public DbSet<ApiScopeProperty> ApiScopeProperties { get; }
        public DbSet<IdentityResource> IdentityResources { get; }
        public DbSet<IdentityResourceClaim> IdentityClaims { get; }
        public DbSet<IdentityResourceProperty> IdentityResourceProperties { get; }
        public DbSet<Client> Clients { get; }
        public DbSet<ClientGrantType> ClientGrantTypes { get; }
        public DbSet<ClientRedirectUri> ClientRedirectUris { get; }
        public DbSet<ClientPostLogoutRedirectUri> ClientPostLogoutRedirectUris { get; }
        public DbSet<ClientScope> ClientScopes { get; }
        public DbSet<ClientSecret> ClientSecrets { get; }
        public DbSet<ClientClaim> ClientClaims { get; }
        public DbSet<ClientIdPRestriction> ClientIdPRestrictions { get; }
        public DbSet<ClientCorsOrigin> ClientCorsOrigins { get; }
        public DbSet<ClientProperty> ClientProperties { get; }
        public DbSet<PersistedGrant> PersistedGrants { get; }
        public DbSet<DeviceFlowCodes> DeviceFlowCodes { get; }

        public IdentityServiceDbContext(DbContextOptions<IdentityServiceDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureIdentity();
            builder.ConfigureIdentityServer();
        }
    }
}