using Mango.Web;
using Mango.Web.Services;
using Mango.Web.Services.IService;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient<IProductService, ProductService>();
var UrlSection = builder.Configuration.GetSection("ServiceUrls");
SD.ProductAPIBase = UrlSection["ProductAPI"];
SD.ShoppingCartAPIBase = UrlSection["ShoppingCartAPI"];
SD.CouponAPIBase = UrlSection["CouponAPI"];
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(option =>
{
    option.DefaultScheme = "Cookies";
    option.DefaultChallengeScheme = "oidc";
}).AddCookie("Cookies", c => c.ExpireTimeSpan = TimeSpan.FromMinutes(10))
.AddOpenIdConnect("oidc", option =>
{
    option.Authority = UrlSection["IdentityAPI"];
    option.GetClaimsFromUserInfoEndpoint = true;
    option.ClientId = "mango";
    option.ClientSecret = "secret";
    option.ResponseType = "code";
    option.ClaimActions.MapJsonKey("role", "role", "role");
    option.ClaimActions.MapJsonKey("sub", "sub", "sub");

    option.TokenValidationParameters.NameClaimType = "name";
    option.TokenValidationParameters.RoleClaimType = "role";
    option.Scope.Add("mango");
    option.SaveTokens = true;
});

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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
