using Api.Common;
using Api.Models.Data;
using Api.Services;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//introducing context
builder.Services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("MyConenctionString")));

//builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));


builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "https://Voxella.com",
        ValidAudience = "https://Voxella.com",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("AbolfazlBarzegar"))
    };
})
.AddCookie(
//    options =>
//{
//    options.LoginPath = "/User/Login";
//    options.LogoutPath = "/User/SignOut";
//    options.ExpireTimeSpan = TimeSpan.FromMinutes(120);
//}
);

builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithOrigins("https://Voxella.com")
            .Build();
    });
});

//register services
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEmailSender,EmailSender>();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseAuthentication();
app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == 401)
    {
        context.Response.ContentType = "application/json; charset=utf-8";
        await context.Response.WriteAsync("{\"status\":false,\"message\":\"invalid token\"}");
    }
});
app.UseAuthorization();
app.UseCors("CorsPolicy");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
  );
});

app.Run();
