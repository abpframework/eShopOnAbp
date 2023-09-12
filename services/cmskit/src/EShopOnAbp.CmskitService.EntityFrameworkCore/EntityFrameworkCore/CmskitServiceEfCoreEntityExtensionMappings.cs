using Volo.Abp.Threading;

namespace EShopOnAbp.CmskitService.EntityFrameworkCore;

public static class CmskitServiceEfCoreEntityExtensionMappings
{
    private static readonly OneTimeRunner OneTimeRunner = new();

    public static void Configure()
    {
        CmskitServiceModuleExtensionConfigurator.Configure();
        OneTimeRunner.Run(() =>
        {
                /* You can configure extra properties for the
                 * entities defined in the modules used by your application.
                 *
                 * This class can be used to map these extra properties to table fields in the database.
                 *
                 * USE THIS CLASS ONLY TO CONFIGURE EF CORE RELATED MAPPING.
                 * USE EShopOnAbpModuleExtensionConfigurator CLASS (in the Domain.Shared project)
                 * FOR A HIGH LEVEL API TO DEFINE EXTRA PROPERTIES TO ENTITIES OF THE USED MODULES
                 *
                 * Example: Map a property to a table field:
                    ObjectExtensionManager.Instance
                        .MapEfCoreProperty<IdentityUser, string>(
                            "SocialSecurityNumber",
                            (entityBuilder, propertyBuilder) =>
                            {
                                propertyBuilder.HasMaxLength(128);
                            }
                        );
                 * See the documentation for more:
                 * https://docs.abp.io/en/abp/latest/Customizing-Application-Modules-Extending-Entities
                 */
        });
    }
}
