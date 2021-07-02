using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;

namespace EShopOnAbp.IdentityService.DbMigrations
{
    public class DataSeederEventHandler : ILocalEventHandler<ApplyDatabaseSeedsEto>, ITransientDependency
    {
        protected IDataSeeder DataSeeder { get; }

        public DataSeederEventHandler(IDataSeeder dataSeeder)
        {
            DataSeeder = dataSeeder;
        }

        public async Task HandleEventAsync(ApplyDatabaseSeedsEto eventData)
        {
            await DataSeeder.SeedAsync();
        }
    }
}
