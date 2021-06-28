using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace EShopOnAbp.SaasService.EntityFrameworkCore
{
    public class SaasServiceModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public SaasServiceModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}