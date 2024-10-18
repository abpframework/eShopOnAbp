using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace EShopOnAbp.PublicWeb.HealthChecks;

public class EShopOnAbpHealthCheck : IHealthCheck, ITransientDependency
{
    protected readonly IIdentityUserAppService IdentityUserAppService;

    public EShopOnAbpHealthCheck(IIdentityUserAppService identityUserAppService)
    {
        IdentityUserAppService = identityUserAppService;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            await IdentityUserAppService.GetListAsync(new GetIdentityUsersInput { MaxResultCount = 1 });

            return HealthCheckResult.Healthy($"Could connect to database and get record.");
        }
        catch (Exception e)
        {
            return HealthCheckResult.Unhealthy($"Error when trying to get database record. ", e);
        }
    }
}
