using EShopOnAbp.WebGateway.Aggregations.Base;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;

namespace EShopOnAbp.WebGateway.Aggregations.Localization;

public interface ILocalizationRemoteService : IAggregateRemoteService<ApplicationLocalizationDto>;