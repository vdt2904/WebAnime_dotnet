using Utf8Json.Formatters;
using Microsoft.AspNetCore.Mvc;
using WebAnime.Models;
using Microsoft.EntityFrameworkCore;
using WebAnime.Repository;
using Microsoft.AspNetCore.Authentication.Google;
using System.Configuration;
using Microsoft.Extensions.Options;
using Hangfire;
using Hangfire.SqlServer;
using Hangfire.AspNetCore;
using Microsoft.Extensions.Configuration;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Authorization;
using WebAnime.Controllers;
using Microsoft.AspNetCore.Http.Extensions; // Thêm namespace này
using Microsoft.AspNetCore.HttpOverrides; // Thêm namespace này
using System.Threading.Tasks; // Thêm namespace này
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectString = builder.Configuration.GetConnectionString("QlAnimeContext");
builder.Services.AddDbContext<QlAnimeContext>(x => x.UseSqlServer(connectString));
builder.Services.AddScoped<ITheLoaiRepository, TheLoaiRepository>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddMvc().AddNewtonsoftJson();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHangfire(config =>
{
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection"));
});
JobStorage.Current = new SqlServerStorage("Data Source=VDT\\SQLEXPRESS;Initial Catalog=QlAnime;Integrated Security=True;TrustServerCertificate=True");
SchedulerController obSchedulerController = new SchedulerController();
RecurringJob.AddOrUpdate(() => obSchedulerController.RunSchedularMethod(), Cron.Daily(00, 00));

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); // Thêm dòng này
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:7277");
                      });
});

var app = builder.Build();
app.UseHangfireServer();
app.UseCors(MyAllowSpecificOrigins);
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
app.UseAuthentication();
app.UseAuthorization();

app.UseForwardedHeaders(); // Thêm dòng này để xử lý tiêu đề chuyển tiếp
app.Use((context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/api"))
    {
        var request = context.Request;
        var targetUrl = "http://thaiutc8424-001-site1.htempurl.com/";
        request.Scheme = "http";
        request.Host = new HostString(targetUrl);
        return next();
    }

    return next();
});

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
