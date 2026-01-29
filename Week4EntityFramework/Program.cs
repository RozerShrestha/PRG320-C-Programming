using Microsoft.EntityFrameworkCore.ChangeTracking;
using Week4EntityFramework.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<IDbInitializer, DbInitializer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//SeedDatabase();
TestingEFFeature();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
app.Run();

void SeedDatabase()
{
    using(var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();
    }
}

void TestingEFFeature()
{
    using (var context = new SchoolContext())
    {
        var student = context.Students.FirstOrDefault();
        context.Students.Add(new Week4EntityFramework.Models.Student() { FirstName = "Hello", LastName = "Gate" });
        var entries = context.ChangeTracker.Entries();
        foreach (var entry in entries)
        {
            Console.WriteLine($"Entity: {entry.Entity.GetType().Name}");
            Console.WriteLine($"State:{entry.State.ToString()}");
        }
    }
}
