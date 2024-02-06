using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;

namespace EShopOnAbp.WebGateway.Aggregations.Localization;

public interface ILocalizationAggregation
{
    string LocalizationRouteName { get; }
    string LocalizationEndpoint { get; }
    Task<ApplicationLocalizationDto> GetLocalizationAsync(LocalizationRequest input);
}