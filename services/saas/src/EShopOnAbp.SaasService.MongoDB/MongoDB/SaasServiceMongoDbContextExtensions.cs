using System;
using Volo.Abp;
using Volo.Abp.MongoDB;

namespace EShopOnAbp.SaasService.MongoDB
{
    public static class SaasServiceMongoDbContextExtensions
    {
        public static void ConfigureSaasService(
            this IMongoModelBuilder builder,
            Action<AbpMongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new SaasServiceMongoModelBuilderConfigurationOptions(
                SaasServiceDbProperties.DbTablePrefix
            );

            optionsAction?.Invoke(options);
        }
    }
}