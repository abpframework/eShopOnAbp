using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.DependencyInjection;

namespace EShopOnAbp.WebGateway.Aggregations;

public interface ILocalizationAggregation : ITransientDependency
{
    public string LocalizationEndpoint { get; }
    public Task<ApplicationLocalizationDto> GetLocalizationAsync(LocalizationRequest input);
}