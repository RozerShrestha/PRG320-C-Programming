using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using BusinessManagementSystem.Data;
using BusinessManagementSystem.Dto;
using BusinessManagementSystem.Helper;
using BusinessManagementSystem.Repositories;
using BusinessManagementSystem.Services;
using BusinessManagementSystem.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using NLog;
using NLog.Web;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;

var mapperConfiguration = new MapperConfiguration(configuration =>
{
    configuration.AddProfile(new MappingProfile());
});

var logger = LogManager.Setup()
    .LoadConfigurationFromFile("nlog.config", optional: true)
    .GetCurrentClassLogger();

var mapper = mapperConfiguration.CreateMapper();
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<ApplicationDBContext>(options => 
{ 
    options.UseSqlServer(builder.Configuration.GetConnectionString("BMSConnection")); 
});
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton(mapper);
builder.Services.AddScoped<ILogin<LoginResponseDto>, LoginRepository>();

builder.Services.AddScoped<BaseRepository>();
builder.Services.AddScoped<IBase>(sp => sp.GetRequiredService<BaseRepository>());

builder.Services.AddScoped<BasicConfigurationRepository>();
builder.Services.AddScoped<IBasicConfiguration>(sp => sp.GetRequiredService<BasicConfigurationRepository>());

builder.Services.AddScoped<MenuRepository>();
builder.Services.AddScoped<IMenu>(sp => sp.GetRequiredService<MenuRepository>());

builder.Services.AddScoped<RoleRepository>();
builder.Services.AddScoped<IRole>(sp => sp.GetRequiredService<RoleRepository>());

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<IUser>(sp => sp.GetRequiredService<UserRepository>());

builder.Services.AddScoped<ITokenService, TokenRepository>();

builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IAuthorizationHandler, RolesAuthorizationHandler>();
builder.Services.AddSingleton<JavaScriptEncoder>(JavaScriptEncoder.Default);
builder.Services.AddRazorPages();


#region JWT Token
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new
         SymmetricSecurityKey
         (Encoding.UTF8.GetBytes
         (builder.Configuration["Jwt:Key"]))
    };
});

#endregion

#region Tostr Notification
builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 5;
    config.IsDismissable = true;
    config.Position = NotyfPosition.TopRight;
    config.HasRippleEffect = true;
});
#endregion

#region Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(12);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
#endregion

#region Logging
builder.Logging.ClearProviders();
builder.Host.UseNLog();
#endregion



var app = builder.Build();
app.Logger.LogInformation("Starting Application");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}
app.UseHttpsRedirection();
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("Content-Security-Policy",
            //"default-src 'self'; " +
            "script-src 'self' 'unsafe-inline' 'unsafe-eval'; "
            //"style-src 'self' 'unsafe-inline'; " +
            //"img-src 'self' data:; " +
            //"font-src 'self'; " +
            //"object-src 'none'; " +
            //"form-action 'self'; " +
            //"base-uri 'self'; " +
            //"frame-ancestors 'none';");
            );
    await next();
});
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.Use((context, next) =>
{
    var token = context.Session.GetString("Token");
    if (!string.IsNullOrEmpty(token))
    {
        context.Request.Headers.Add("Authorization", "Bearer " + token);
    }
    return next();
});
SeedDatabase();
app.UseAuthentication();
app.UseStatusCodePages(context =>
{
    var response = context.HttpContext.Response;
    if (response.StatusCode == (int)HttpStatusCode.Unauthorized)
    {
        app.Logger.LogWarning("Unauthrorized");
        response.Redirect("/");
    }
    else if (response.StatusCode == (int)HttpStatusCode.Forbidden)
    {
        app.Logger.LogWarning("Forbidden");
        response.Redirect("/Users/AccessDenied");
    }
    else if (response.StatusCode == (int)HttpStatusCode.NotFound)
    {
        app.Logger.LogWarning("NotFound");
        response.Redirect("/Error/PageNotFound");
    }

    return Task.CompletedTask;
});
app.UseAuthorization();
app.MapControllers();
app.UseNotyf();
//app.MapRazorPages();
app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Login}/{action=index}/{id?}");

app.Run();

void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();
    }
}
 