using FinalProject.Back.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// add connection string to the configuration
builder.Services.AddDbContext<CertificationDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
	{
		Description = "Please enter token",
		In = ParameterLocation.Header,
		Name = "Authorization",
		Type = SecuritySchemeType.ApiKey
	});
	options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddCors(options =>
{
	options.AddPolicy("CorsPolicy", policy =>
	{
		policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:7273");
	});
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddCookie(x =>
	{
		x.Cookie.Name = "token";
	}).AddJwtBearer
	(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
			ValidAudience = builder.Configuration["JwtSettings:Audience"],
			IssuerSigningKey = new SymmetricSecurityKey
			(
				Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!)
			)

		};
	}
	);



builder.Services.AddAuthentication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("CorsPolicy"); // Apply the CORS policy


app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
