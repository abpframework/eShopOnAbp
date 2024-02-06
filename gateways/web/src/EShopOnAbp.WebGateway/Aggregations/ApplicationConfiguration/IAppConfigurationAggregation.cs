using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;

namespace EShopOnAbp.WebGateway.Aggregations.ApplicationConfiguration;

public interface IAppConfigurationAggregation
{
    string RouteName { get; }
    string Endpoint { get; }
    Task<ApplicationConfigurationDto> GetAsync(AppConfigurationRequest input);
}