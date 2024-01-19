using FinalProject.Front.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FinalProject.Front.Pages
{

	[Route("/AdminHome")]
	public class AdminHomeModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;
		public readonly IContextHelper _contextHelper;

		public AdminHomeModel(ILogger<IndexModel> logger, IContextHelper contextHelper)
		{
			_logger = logger;
			_contextHelper = contextHelper;
		}

		public async Task<IActionResult> OnGet()
		{
			if (!User.Identity.IsAuthenticated)
			{
				return Redirect("/Account/Login");
			}
			else
			{
				if (!User.IsInRole("admin"))
				{
					return Redirect("/AccessDenied");
				}
			}
			return Page();
		}
	}
}
