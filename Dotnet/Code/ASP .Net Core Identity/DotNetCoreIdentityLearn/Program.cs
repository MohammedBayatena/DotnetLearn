using DotNetCoreIdentityLearn.DataBase;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

#region Configure Identity Service

//Add the DBContext
builder
    .Services
    .AddDbContext<ApplicationDbContext>(opt =>
    {
        opt.UseInMemoryDatabase(databaseName: "Identity");
    });

//Add Identity Service
builder.Services
    .AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddApiEndpoints();

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGroup("/account").MapIdentityApi<IdentityUser>().WithTags("IdentityServer");

app.Run();