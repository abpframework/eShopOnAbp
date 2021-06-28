using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace EShopOnAbp.SaasService.Pages
{
    public class IndexModel : SaasServicePageModel
    {
        public void OnGet()
        {
            
        }

        public async Task OnPostLoginAsync()
        {
            await HttpContext.ChallengeAsync("oidc");
        }
    }
}