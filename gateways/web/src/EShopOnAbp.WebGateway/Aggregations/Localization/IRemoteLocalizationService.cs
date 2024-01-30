using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;

namespace EShopOnAbp.WebGateway.Aggregations.Localization;

public interface IRemoteLocalizationService
{
    Task<Dictionary<string, ApplicationLocalizationDto>> GetMultipleLocalizationsAsync(Dictionary<string, string> serviceNameWithUrlDictionary);
}