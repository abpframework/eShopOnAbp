using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace EShopOnAbp.PublicWeb.Pages;

public class BasketModel : AbpPageModel
{
    private readonly ILogger<BasketModel> _logger;

    public BasketModel(ILogger<BasketModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }

    public IActionResult OnPostAsync()
    {
        if (!CurrentUser.IsAuthenticated)
        {
            _logger.LogInformation("Redirecting to Login");
            return Challenge();
        }

        return RedirectToPage("Payment");
    }
}