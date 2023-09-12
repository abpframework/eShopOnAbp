using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using IdentityRole = Volo.Abp.Identity.IdentityRole;

namespace EShopOnAbp.IdentityService.DbMigrations;

public class IdentityPublicRoleDataSeeder : IDataSeedContributor, ITransientDependency
{
    private readonly IIdentityRoleRepository _roleRepository;
    private readonly ILookupNormalizer _lookupNormalizer;
    private readonly IGuidGenerator _guidGenerator;
    private readonly IdentityRoleManager _roleManager;

    public IdentityPublicRoleDataSeeder(
        IIdentityRoleRepository roleRepository,
        ILookupNormalizer lookupNormalizer,
        IGuidGenerator guidGenerator,
        IdentityRoleManager roleManager)
    {
        _roleRepository = roleRepository;
        _lookupNormalizer = lookupNormalizer;
        _guidGenerator = guidGenerator;
        _roleManager = roleManager;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        await CreateCustomerRoleAsync();
    }

    private async Task CreateCustomerRoleAsync()
    {
        //"customer" role
        var customerRole =
            await _roleRepository.FindByNormalizedNameAsync(_lookupNormalizer.NormalizeName(IdentityServiceDbProperties.CustomerRoleName));
        if (customerRole == null)
        {
            customerRole = new IdentityRole(_guidGenerator.Create(), IdentityServiceDbProperties.CustomerRoleName)
            {
                IsStatic = true,
                IsPublic = true
            };

            (await _roleManager.CreateAsync(customerRole)).CheckErrors();
        }
    }
}