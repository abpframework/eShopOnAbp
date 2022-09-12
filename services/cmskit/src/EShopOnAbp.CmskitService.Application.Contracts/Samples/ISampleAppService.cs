using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace EShopOnAbp.CmskitService.Samples;

public interface ISampleAppService : IApplicationService
{
    Task<SampleDto> GetAsync();

    Task<SampleDto> GetAuthorizedAsync();
}
