using FinalProject.Front.Helpers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace FinalProject.Front.Pages
{
    public class IndexModel : PageModel
    {

        private readonly IContextHelper _contextHelper;

        public IndexModel(IContextHelper contextHelper)
        {

            _contextHelper = contextHelper;
        }

        public void OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                _contextHelper.Token = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            }
        }
    }
}
