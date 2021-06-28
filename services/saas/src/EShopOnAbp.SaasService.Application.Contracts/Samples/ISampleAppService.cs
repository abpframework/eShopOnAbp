using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace EShopOnAbp.SaasService.Samples
{
    public interface ISampleAppService : IApplicationService
    {
        Task<SampleDto> GetAsync();

        Task<SampleDto> GetAuthorizedAsync();
    }
}
