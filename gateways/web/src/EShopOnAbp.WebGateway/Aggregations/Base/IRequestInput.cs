using System.Collections.Generic;

namespace EShopOnAbp.WebGateway.Aggregations.Base;

public interface IRequestInput
{
    Dictionary<string, string> Endpoints { get; }
}