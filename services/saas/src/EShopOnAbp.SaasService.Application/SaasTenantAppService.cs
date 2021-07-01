using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectExtending;
using Volo.Abp.TenantManagement;

namespace EShopOnAbp.SaasService
{
    [Dependency(ReplaceServices = true)]
    public class SaasTenantAppService : TenantAppService, ITenantAppService
    {
        protected IDistributedEventBus DistributedEventBus { get; }

        public SaasTenantAppService(
            ITenantRepository tenantRepository,
            ITenantManager tenantManager,
            IDataSeeder dataSeeder,
            IDistributedEventBus distributedEventBus) : base(tenantRepository, tenantManager, dataSeeder)
        {
            DistributedEventBus = distributedEventBus;
        }

        public override async Task<TenantDto> CreateAsync(TenantCreateDto input)
        {
            var tenant = await TenantManager.CreateAsync(input.Name);
            input.MapExtraPropertiesTo(tenant);

            await TenantRepository.InsertAsync(tenant);

            await CurrentUnitOfWork.SaveChangesAsync();

            await DistributedEventBus.PublishAsync(new TenantCreatedEto
            {
                Id = tenant.Id,
                Name = tenant.Name,
                Properties = new Dictionary<string, string>
                {
                    { "AdminEmail", input.AdminEmailAddress },
                    { "AdminPassword", input.AdminPassword }
                }
            });

            return ObjectMapper.Map<Tenant, TenantDto>(tenant);
        }
    }
}
