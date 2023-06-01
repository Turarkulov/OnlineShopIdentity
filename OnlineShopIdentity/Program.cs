using System.IO;
using OnlineShopIdentity.Models;
using OnlineShopIdentity;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using OnlineShopIdentity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.AspNetCore.Identity;
using System.Data.Common;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

//var host=builder.Build();
//using (var scope = host.Services.CreateScope())
//{
//    var serviseProvider = scope.ServiceProvider;
//    try
//    {
//        var context = serviseProvider.GetRequiredService<AuthDbContext>();
//        DbInitializer.Initialize(context);
//    }
//    catch (Exception exception)
//    {
//        var logger = serviseProvider.GetRequiredService<ILogger<Program>>();
//        logger.LogError(exception, "An error occurred while app initialization");
//    }
//}
//host.Run();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add connectionString
builder.Services.AddDbContext<AuthDbContext>(
   options =>
{
    options.UseSqlite();
    
});
builder.Services.AddIdentity<AppUser, IdentityRole>(config =>
{
    config.Password.RequiredLength = 4;
    config.Password.RequireDigit = false;
    config.Password.RequireNonAlphanumeric = false;
    config.Password.RequireUppercase = false;
})
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddIdentityServer()
    .AddAspNetIdentity<AppUser>()
    .AddInMemoryApiResources(Configuration.ApiResources)
    .AddInMemoryIdentityResources(Configuration.IdentityResources)
    .AddInMemoryApiScopes(Configuration.ApiScopes)
    .AddInMemoryClients(Configuration.Clients)
    .AddDeveloperSigningCredential();

builder.Services.ConfigureApplicationCookie(config =>
{
    config.Cookie.Name = "OnlineShop.Identity.Cookie";
    config.LoginPath = "/Auth/Login";
    config.LogoutPath = "/Auth/Logout";
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
}
//app.UseStaticFiles(new StaticFileOptions
//{
//FileProvider = new PhysicalFileProvider(
//    Path.Combine("Styles")),
//RequestPath = "/styles"
//});
app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();
app.UseIdentityServer();
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllers();
//});

app.Run();
