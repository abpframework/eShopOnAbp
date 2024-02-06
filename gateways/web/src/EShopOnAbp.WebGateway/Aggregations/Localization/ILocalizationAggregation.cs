using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;

namespace EShopOnAbp.WebGateway.Aggregations.Localization;

public interface ILocalizationAggregation
{
    string RouteName { get; }
    string Endpoint { get; }
    Task<ApplicationLocalizationDto> GetAsync(LocalizationRequest input);
}