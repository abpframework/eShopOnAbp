using System.Threading.Tasks;
using EShopOnAbp.DbMigrator.Keycloak;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace EShopOnAbp.DbMigrator;

public class KeyCloakDataSeeder : IDataSeedContributor, ITransientDependency
{
    private readonly KeycloakService _keycloakService;

    public KeyCloakDataSeeder(KeycloakService keycloakService)
    {
        _keycloakService = keycloakService;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        var result = await _keycloakService.GetAdminAccessTokenAsync("master");
    }
}