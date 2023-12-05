using System.Diagnostics;
using System.Runtime.InteropServices;
using BinanceBotNuGetPackage.DB.DbContext;
using BinanceBotNuGetPackage.Helpers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var connectionString = @"Data Source=" + Path.Combine(Directory.GetCurrentDirectory(), "BinanceBotDb.db");

FinderHelper.CheckIfPeriodsExist(connectionString);
FinderHelper.RemoveDuplicates(connectionString);


var app = builder.Build();


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
    pattern: "{controller=InputForm}/{action=Index}/{id?}");

if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
    Process.Start(new ProcessStartInfo("http://localhost:5000") { UseShellExecute = true });
}
else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
{
    Process.Start("xdg-open", "http://localhost:5000");
}

app.Run();

