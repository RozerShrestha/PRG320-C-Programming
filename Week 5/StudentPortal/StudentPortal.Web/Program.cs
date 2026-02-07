using Microsoft.EntityFrameworkCore;
using StudentPortal.Web.Data;

var builder = WebApplication.CreateBuilder(args);
//Here we add services
// Add services to the container.
builder.Services.AddControllersWithViews();
//This is how we inject ApplicationDbContext and we are telling application to use sql server database
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("StudentPortal")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//this is default Route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
