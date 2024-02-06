using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EShopOnAbp.WebGateway.Aggregations.Base;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.DependencyInjection;

namespace EShopOnAbp.WebGateway.Aggregations.ApplicationConfiguration;

public class AppConfigurationAggregation : AggregateServiceBase<ApplicationConfigurationDto>,
    IAppConfigurationAggregation,
    ITransientDependency
{
    public string RouteName => "EshopOnAbpApplicationConfiguration";
    public string Endpoint => "api/abp/application-configuration";


    protected AppConfigurationCachedService AppConfigurationCachedService { get; }

    public AppConfigurationAggregation(
        AppConfigurationCachedService appConfigurationCachedService,
        IAppConfigurationRemoteService appConfigurationRemoteService) : base(
        appConfigurationRemoteService)
    {
        AppConfigurationCachedService = appConfigurationCachedService;
    }

    public async Task<ApplicationConfigurationDto> GetAsync(AppConfigurationRequest input)
    {
        // Check the cache service
        var cachedAppConfiguration = AppConfigurationCachedService
            .GetManyAsync(input.Endpoints.Keys.ToArray());

        // Compare cache with input service list
        var missingAppConfigurations = GetMissingServiceKeys(cachedAppConfiguration, input.Endpoints);

        if (missingAppConfigurations.Count != 0)
        {
            // Make request to remote appConfiguration service to get missing localizations
            var remoteAppConfigurationResults =
                await GetMultipleFromRemoteAsync(missingAppConfigurations, input.Endpoints);

            // Update appConfiguration cache
            foreach (var result in remoteAppConfigurationResults)
            {
                AppConfigurationCachedService.Add(result.Key, result.Value);
            }

            cachedAppConfiguration = AppConfigurationCachedService
                .GetManyAsync(input.Endpoints.Keys.ToArray());
        }

        //merge only application configuration settings data
        var mergedResult = MergeAppConfigurationSettingsData(cachedAppConfiguration);

        //return result
        return mergedResult;
    }

    private static ApplicationConfigurationDto MergeAppConfigurationSettingsData(
        IDictionary<string, ApplicationConfigurationDto> appConfigurations)
    {
        ApplicationConfigurationDto appConfigurationDto = CreateInitialAppConfigDto(appConfigurations);

        foreach (var (_, appConfig) in appConfigurations)
        {
            foreach (var resource in appConfig.Setting.Values)
            {
                appConfigurationDto.Setting.Values.TryAdd(resource.Key, resource.Value);
            }
        }

        return appConfigurationDto;
    }

    /// <summary>
    /// Checks "Administration" clusterId to set the initial data from the AdministrationService.
    /// Otherwise uses the first available service for the initial application configuration data 
    /// </summary>
    /// <param name="appConfigurations"></param>
    /// <returns></returns>
    private static ApplicationConfigurationDto CreateInitialAppConfigDto(
        IDictionary<string, ApplicationConfigurationDto> appConfigurations
    )
    {
        if (appConfigurations.Count == 0)
        {
            return new ApplicationConfigurationDto();
        }

        var exist = appConfigurations.ContainsKey("Administration_AppConfig");
        var dodo = appConfigurations["Administration_AppConfig"];
        
        if(appConfigurations.TryGetValue("Administration_AppConfig", out var administrationServiceData))
        {
            return MapServiceData(administrationServiceData);
        }

        return MapServiceData(appConfigurations.First().Value);
    }

    private static ApplicationConfigurationDto MapServiceData(ApplicationConfigurationDto appConfiguration)
    {
        return new ApplicationConfigurationDto
        {
            Localization = appConfiguration.Localization,
            Auth = appConfiguration.Auth,
            Clock = appConfiguration.Clock,
            Setting = appConfiguration.Setting,
            Features = appConfiguration.Features,
            Timing = appConfiguration.Timing,
            CurrentTenant = appConfiguration.CurrentTenant,
            CurrentUser = appConfiguration.CurrentUser,
            ExtraProperties = appConfiguration.ExtraProperties,
            GlobalFeatures = appConfiguration.GlobalFeatures,
            MultiTenancy = appConfiguration.MultiTenancy,
            ObjectExtensions = appConfiguration.ObjectExtensions
        };
    }
}