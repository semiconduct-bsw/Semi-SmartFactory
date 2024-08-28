using Microsoft.AspNetCore.Mvc;
using System;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute(
    name: "dafault",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
