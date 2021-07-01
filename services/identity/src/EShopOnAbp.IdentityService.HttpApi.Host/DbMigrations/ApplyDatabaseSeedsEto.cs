using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus;

namespace EShopOnAbp.IdentityService.DbMigrations
{
    [EventName("abp.data.apply_database_seeds")]
    public class ApplyDatabaseSeedsEto : EtoBase
    {
    }
}
