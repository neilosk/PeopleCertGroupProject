using FinalProject.Data.Dtos;
using FinalProject.Front.Helpers;
using FinalProject.Front.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FinalProject.Front.Pages
{

	public class GetUsersModel : PageModel
	{

		private readonly AccountService _accountService;
		private readonly IContextHelper _contextHelper;

		public List<UserProfileDto> Users { get; private set; }
		public GetUsersModel(AccountService accountService, IContextHelper contextHelper)
		{
			_accountService = accountService;
			_contextHelper = contextHelper;
		}
		public async Task<IActionResult> OnGet()
		{
			if (User.Identity.IsAuthenticated)
			{
				if (!User.IsInRole("admin"))
				{
					return Redirect("/AccessDenied");
				}
				else
				{
					Users = await _accountService.GetAccounts();
					return Page();
				}
			}
			return Redirect("/Account/Login");
		}

	}
}
