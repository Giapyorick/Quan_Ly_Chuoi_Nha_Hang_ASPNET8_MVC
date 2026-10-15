using GiapDepTrai.Models;
using Microsoft.EntityFrameworkCore;
using GiapDepTrai.Hubs;



var builder = WebApplication.CreateBuilder(args);
var conn = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<QlChuoiNhaHang1Context>(options => options.UseSqlServer(conn));
builder.Services.AddDistributedSqlServerCache(options =>
{
    options.ConnectionString = builder.Configuration.GetConnectionString("Default");
    options.SchemaName = "dbo";
    options.TableName = "SessionState";
});
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1); 
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddSignalR();
builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });



var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();

app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value?.ToLower();

    if (path == null || 
        path.StartsWith("/css") || 
        path.StartsWith("/js") || 
        path.StartsWith("/lib") || 
        path.StartsWith("/kitchenhub") || 
        path.StartsWith("/admin/login/login"))
    {
        await next();
        return;
    }

    if (path.StartsWith("/admin") && context.Session.GetInt32("EmployeeID") == null)
    {
        Console.WriteLine($"Redirect to /Admin/Login/Login from {path}"); 
        context.Response.Redirect("/Admin/Login/Login?returnUrl=" + Uri.EscapeDataString(context.Request.Path + context.Request.QueryString));
        return;
    }

    await next();
});

app.UseAuthorization();

app.MapHub<KitchenHub>("/kitchenHub");

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();