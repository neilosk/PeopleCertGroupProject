using FinalProject.Data.Dtos;
using FinalProject.Front.Helpers;
using FinalProject.Front.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace FinalProject.Front.Pages
{
	public class LoginModel : PageModel
	{

		private readonly AccountService _accountService;
		private readonly IContextHelper _contextHelper;

		[BindProperty]
		public LoginDto Input { get; set; }
		public LoginModel(AccountService accountService, IContextHelper contextHelper)
		{
			_accountService = accountService;
			_contextHelper = contextHelper;
		}



		public async Task OnGet()
		{
		}

		// 

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			try
			{
				// Call your API to authenticate the user
				var result = await _accountService.LoginAsync(Input.Email, Input.Password);



				if (result.IsSuccess)
				{


					var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Name,result.User.FirstName+" "+result.User.LastName ),
						new Claim(ClaimTypes.Role, result.User.Role!),
						new Claim(ClaimTypes.Hash, result.Token),
						new Claim(ClaimTypes.Email, result.User.Email),
                        
                        // You can add other claims as needed
                    };

					var claimsIdentity = new ClaimsIdentity(claims, "CookieAuthentication");
					var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
					await HttpContext.SignInAsync("CookieAuthentication", claimsPrincipal);

					// Redirect to the appropriate page after successful login
					if (result.User.Role == "admin")
					{
						return Redirect("/AdminHome");
					}
					else
					{
						return Redirect("/");
					}
				}
				else
				{
					// Handle failed login attempt (e.g., show error message)
					ModelState.AddModelError(string.Empty, "Invalid login attempt.");
					return Page();
				}
			}
			catch (Exception ex)
			{

				// Handle exceptions (e.g., show error message)
				ModelState.AddModelError(string.Empty, "An error occurred during login.");
				return Page();
			}
		}
	}
}
