using Quipu_Task.Helpers;
using Microsoft.EntityFrameworkCore;
using Quipu_Task.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DataContext>(
        options => options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ClientInfo;Trusted_Connection=True;MultipleActiveResultSets=true"));
builder.Services.AddScoped<IClientService, ClientService>();
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


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ClientInfoes}/{action=Index}/{id?}");

app.Run();
