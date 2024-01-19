using FinalProject.Front.Helpers;
using FinalProject.Front.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddHttpClient<AccountService>(client =>
{
	client.BaseAddress = new Uri("https://localhost:7193");
});

builder.Services.AddSingleton<IContextHelper, ContextHelper>();

builder.Services.AddAuthentication("CookieAuthentication")
		.AddCookie("CookieAuthentication", options =>
		{

			options.LoginPath = "/Login"; // Your login path
			options.AccessDeniedPath = "/Error"; // Path for access denied
			options.LogoutPath = "/";
		});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapRazorPages();

app.MapControllers();

app.Run();
