using Volo.Abp.GlobalFeatures;
using Volo.Abp.Threading;

namespace EShopOnAbp.CmskitService;

public static class FeatureConfigurer
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public static void Configure()
    {
        OneTimeRunner.Run(() =>
        {
            GlobalFeatureManager.Instance.Modules.CmsKit().Comments.Enable();
            GlobalFeatureManager.Instance.Modules.CmsKit().Ratings.Enable();
        });
    }
}

