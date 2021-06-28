using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace EShopOnAbp.SaasService.MongoDB
{
    public class SaasServiceMongoModelBuilderConfigurationOptions : AbpMongoModelBuilderConfigurationOptions
    {
        public SaasServiceMongoModelBuilderConfigurationOptions(
            [NotNull] string collectionPrefix = "")
            : base(collectionPrefix)
        {
        }
    }
}