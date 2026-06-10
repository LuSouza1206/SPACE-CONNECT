using Microsoft.AspNetCore.Authentication.Cookies;
using SpaceConnect.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
});

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(opt =>
{
    opt.SerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
});

var apiBase = builder.Configuration["services:apiservice:0"]
    ?? builder.Configuration["ApiService:BaseUrl"]
    ?? "http://localhost:5001";
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<ApiAuthHandler>();
builder.Services.AddHttpClient<ApiService>(c => c.BaseAddress = new Uri(apiBase))
    .AddHttpMessageHandler<ApiAuthHandler>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opt =>
    {
        opt.LoginPath = "/Auth/Login";
        opt.LogoutPath = "/Auth/Logout";
        opt.AccessDeniedPath = "/Auth/AcessoNegado";
        opt.ExpireTimeSpan = TimeSpan.FromHours(8);
        opt.Cookie.Name = "SpaceConnect.Auth";
    });

builder.Services.AddSession();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.Use(async (context, next) =>
{
    await next();
    if (context.Response.ContentType?.StartsWith("text/html", StringComparison.OrdinalIgnoreCase) == true
        && context.Response.ContentType?.Contains("charset", StringComparison.OrdinalIgnoreCase) != true)
    {
        context.Response.ContentType = "text/html; charset=utf-8";
    }
});

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
