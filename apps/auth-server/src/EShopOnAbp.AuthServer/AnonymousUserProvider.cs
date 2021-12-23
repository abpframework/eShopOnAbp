using Volo.Abp.DependencyInjection;

namespace EShopOnAbp.AuthServer;

public class AnonymousUserProvider : ISingletonDependency
{
    public string AnonymousUserId { get; set; }
}