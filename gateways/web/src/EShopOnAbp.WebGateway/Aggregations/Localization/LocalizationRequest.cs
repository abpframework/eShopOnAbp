using System.Collections.Generic;
using EShopOnAbp.WebGateway.Aggregations.Base;

namespace EShopOnAbp.WebGateway.Aggregations.Localization;

public class LocalizationRequest : IRequestInput
{
    public Dictionary<string, string> Endpoints { get; } = new();
    public string CultureName { get; set; }

    public LocalizationRequest(string cultureName)
    {
        CultureName = cultureName;
    }
}