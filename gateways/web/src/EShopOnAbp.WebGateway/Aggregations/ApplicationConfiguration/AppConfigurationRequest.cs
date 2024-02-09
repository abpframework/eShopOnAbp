using System.Collections.Generic;
using EShopOnAbp.WebGateway.Aggregations.Base;

namespace EShopOnAbp.WebGateway.Aggregations.ApplicationConfiguration;

public class AppConfigurationRequest : IRequestInput
{
    public Dictionary<string, string> Endpoints { get; } = new();
}