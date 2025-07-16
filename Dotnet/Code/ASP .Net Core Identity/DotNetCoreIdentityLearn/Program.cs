using DotNetCoreIdentityLearn.DataBase;
using DotNetCoreIdentityLearn.DataBase.DataSeeders;
using DotNetCoreIdentityLearn.DataBase.Entities;
using DotNetCoreIdentityLearn.Helpers.Extensions;
using DotNetCoreIdentityLearn.Options;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Register Path Options
var defaultPaths = builder.Configuration.GetSection(DefaultPathsOptions.DefaultPaths);
builder.Services.Configure<DefaultPathsOptions>(defaultPaths);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policyBuilder =>
    {
        policyBuilder.WithOrigins("https://localhost:7258")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});


#region Register Custom Services

builder.Services.AddScoped<DefaultDataSeeder, DefaultDataSeeder>();

#endregion

#region Configure Identity Service

//Add the DBContext
builder
    .Services
    .AddDbContext<ApplicationDbContext>(opt => { opt.UseInMemoryDatabase(databaseName: "Identity"); });

//Add Identity Service
builder.Services
    .AddIdentity<ApplicationUser, ApplicationRole>(o => { o.AddPasswordConfiguration(); })
    .AddEntityFrameworkStores<ApplicationDbContext>();

#endregion

#region Configure Application Cookies

builder.Services.ConfigureApplicationCookie(o =>
{
    o.LoginPath = defaultPaths["LoginPathUrl"];
    o.AccessDeniedPath = defaultPaths["AccessDeniedPathUrl"];
});

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    //Try Seed Default Data
    using var scope = app.Services.CreateScope();
    var defaultDataSeeder = scope.ServiceProvider.GetRequiredService<DefaultDataSeeder>();
    await defaultDataSeeder.SeedAsync();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseStatusCodePagesWithReExecute("/Error/{0}");
app.UseCors("AllowSpecificOrigin");

//Configuring Authentication Middleware to the Request Pipeline
app.UseAuthentication();
app.UseAuthorization();

//MVC Middleware
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();