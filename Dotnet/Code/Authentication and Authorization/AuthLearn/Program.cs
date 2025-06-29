using System.Globalization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using AuthLearn.AuthenticationHandlers;
using AuthLearn.Constants;
using AuthLearn.Database;
using AuthLearn.Entities;
using AuthLearn.JWTAuthentication;
using AuthLearn.Options;
using AuthLearn.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

// Add services to the container.
builder.Services.AddControllers();
// builder.Services.AddDataProtection();
// builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase(databaseName: "AuthLearn"));

//Creating Options Pattern for JWTConfigs
var jwtConfig = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettingsConfig>(jwtConfig);


const string localAuthScheme = AuthSchemesConstants.LocalCookieScheme;
const string visitorAuthScheme = AuthSchemesConstants.VisitorCookieScheme;
const string oAuthCookieScheme = AuthSchemesConstants.OAuthCookieScheme;
const string mockOAuthScheme = AuthSchemesConstants.MockOAuthScheme;
const string jwtScheme = AuthSchemesConstants.JwtScheme;
const string githubOAuthScheme = AuthSchemesConstants.GithubOAuthScheme;

// Create RSA Key
// KeyGenerator.GenerateKey();

#region Register Authentication

builder.Services
    .AddAuthentication(visitorAuthScheme)
    .AddScheme<CookieAuthenticationOptions,
        VisitorAuthenticationHandler>(visitorAuthScheme, _ => { }) //Manual Handled Scheme issued directly
    .AddCookie(localAuthScheme, o =>
    {
        o.Events.OnValidatePrincipal = async context =>
        {
            var blacklistedSessionsRepository =
                context.HttpContext.RequestServices.GetRequiredService<IBlacklistedSessionsRepository>();
            var sessionId = context.Principal?.FindFirstValue(ClaimTypes.Sid);
            if (await blacklistedSessionsRepository.ExistsAsync(sessionId))
                context.RejectPrincipal();
        };

        //Set Response as 401 instead of redirecting to login page when Scheme Challenge is called
        o.Events.OnRedirectToLogin = context =>
        {
            context.Response.Headers.Location = context.RedirectUri;
            context.Response.StatusCode = 401;
            return Task.CompletedTask;
        };
        o.LoginPath = "/login"; //Dummy Path Should not be used to enter data
        o.AccessDeniedPath = "/accessdenied";
    }) // Local Cookie Scheme used after login from Cookies controller
    .AddJwtBearer(jwtScheme, o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters()
        {
            AuthenticationType = jwtScheme,
            ValidIssuer = config["JwtSettings:Issuer"],
            ValidAudience = config["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:Key"])),
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true
        };
        o.MapInboundClaims = false; //Stops Dotnet From mapping Custom Types into the built-in ClaimTypes Class
    })
    .AddCookie(oAuthCookieScheme)
    .AddOAuth(mockOAuthScheme, o =>
    {
        //Save The Token in the Scheme Provided for SignInScheme
        o.SignInScheme = oAuthCookieScheme; // We will save this in a cookie just for OAuth

        o.ClientId = "Id";
        o.ClientSecret = "secret";

        //Mock Server
        o.AuthorizationEndpoint = "https://oauth.wiremockapi.cloud/oauth/authorize";
        o.TokenEndpoint = "https://oauth.wiremockapi.cloud/oauth/token";
        o.UserInformationEndpoint = "https://oauth.wiremockapi.cloud/userinfo";
        o.CallbackPath = "/OAuthCallback";

        o.Scope.Add("Oauth");
        o.SaveTokens = true;
    })
    .AddOAuth(githubOAuthScheme, options =>
    {
        var configurationSection = builder.Configuration.GetSection("Authentication:GitHub");

        //This the internal auth schema
        //that will save claims about the user
        //without this schema Oauth would not work
        options.SignInScheme = oAuthCookieScheme;

        options.ClientId = configurationSection["ClientId"];
        options.ClientSecret = configurationSection["ClientSecret"];
        options.CallbackPath = configurationSection["CallbackPath"];
        options.AuthorizationEndpoint = configurationSection["AuthorizationEndpoint"];
        options.TokenEndpoint = configurationSection["TokenEndpoint"];
        options.UserInformationEndpoint = configurationSection["UserInformationEndpoint"];

        options.Scope.Add("read:user");
        options.Scope.Add("user:email");

        //Saves the Access Tokens inside the Internal Auth Scheme
        options.SaveTokens = true;

        //Mapping Claims into types , using Claim Actions
        options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
        options.ClaimActions.MapJsonKey(ClaimTypes.Name, "login");
        options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
        options.ClaimActions.MapJsonKey("urn:github:url", "html_url");
        options.ClaimActions.MapJsonKey("urn:github:avatar", "avatar_url");

        options.Events = new OAuthEvents
        {
            OnCreatingTicket = async context =>
            {
                //Create a request for the UserInformationEndpoint
                using var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);

                //Attach the Access Token acquired from previous step 
                //into the request header
                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", context.AccessToken);

                // Define the application we are accessing the information
                //TheApplicationNameSetWhenRegisteringtWithGithubAuthServer
                request.Headers.UserAgent.Add(
                    new ProductInfoHeaderValue("ApplicationName", "1.0"));

                //Send the Request using the Backchannel
                using var response = await context.Backchannel.SendAsync(
                    request,
                    HttpCompletionOption.ResponseHeadersRead,
                    context.HttpContext.RequestAborted);

                response.EnsureSuccessStatusCode();

                var user = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

                context.RunClaimActions(user.RootElement);
            }
        };
    });

#endregion

#region Register Authorization

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("IsAuthenticated", pol =>
    {
        pol.AddAuthenticationSchemes(localAuthScheme, visitorAuthScheme, mockOAuthScheme, jwtScheme, githubOAuthScheme);
        pol.RequireAuthenticatedUser();
    })
    .AddPolicy("IsAuthenticatedWithLocalCookieScheme", pol =>
    {
        pol.AddAuthenticationSchemes(localAuthScheme);
        pol.RequireAuthenticatedUser();
    })
    .AddPolicy("IsAdmin", pol =>
    {
        pol.AddAuthenticationSchemes(localAuthScheme);
        pol.RequireAuthenticatedUser();
        pol.RequireAssertion(context => context.User.IsInRole("Admin"));
    })
    .AddPolicy("IsOlderThan18", pol =>
    {
        pol.AddAuthenticationSchemes(localAuthScheme);
        pol.RequireAuthenticatedUser();
        pol.RequireAssertion(context => context.User.HasClaim(c => c.Type == ClaimTypes.DateOfBirth) && DateTime
            .Compare(
                DateTime.Parse(context.User.FindFirst(c => c.Type == ClaimTypes.DateOfBirth)!.Value,
                    CultureInfo.InvariantCulture),
                DateTime.Now.AddYears(-18)) < 0
        );
    })
    .AddPolicy("IsAuthenticatedWithOAuth", pol =>
    {
        pol.AddAuthenticationSchemes(mockOAuthScheme);
        pol.RequireAuthenticatedUser();
    })
    .AddPolicy("IsAuthenticatedWithGitHub", pol =>
    {
        pol.AddAuthenticationSchemes(githubOAuthScheme);
        pol.RequireAuthenticatedUser();
    })
    .AddPolicy("IsAuthenticatedWithJwt", pol =>
    {
        pol.AddAuthenticationSchemes(jwtScheme);
        pol.RequireAuthenticatedUser();
    });

#endregion

#region Dependency Injection Registration

//Registers
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IRefreshTokensRepository, RefreshRefreshTokensRepository>();
builder.Services.AddScoped<IBlacklistedSessionsRepository, BlacklistedSessionsRepository>();
builder.Services.AddScoped<IBlacklistedTokensRepository, BlacklistedTokensRepository>();
builder.Services.AddScoped<IPermissionsRepository,PermissionsRepository>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
// builder.Services.AddScoped<ILoginService, LoginService>();

#endregion

#region Swagger Registration

// Add Swagger no JWT
//builder.Services.AddSwaggerGen();

//Add JWT Swagger
builder.Services.AddSwaggerGen(o =>
{
    o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        In = ParameterLocation.Header,
        Description = "Enter Token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    o.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

#endregion

builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();

#region Simple Auth by adjusting the middleware (Obselete)

// app.Use((ctx, next) =>
// {
//     
//     //Skip the Login Path from Auth
//     if(ctx.Request.Path == new PathString("/api/login"))
//         return next();
//     
//     
//     // If no Cookie Found -> Set Response Code to Unauthorized -> Continue Next MiddleWare
//     ctx.Request.Cookies.TryGetValue("UserName", out var userName);
//     if (userName is null)
//     {
//         ctx.Response.StatusCode = 401;
//         return Task.CompletedTask;
//     }
//
//     // If a Cookie Found -> Set User Claims -> Continue Next MiddleWare
//
//     var dataProtectionProvider = ctx.RequestServices.GetRequiredService<IDataProtectionProvider>();
//     var protector = dataProtectionProvider.CreateProtector("Auth-Cookie");
//     var decryptedPayload = protector.Unprotect(userName);
//
//     //Create an Identity for the User
//     var identity = new ClaimsIdentity();
//     identity.AddClaims([
//         new Claim(ClaimTypes.Name, decryptedPayload),
//         new Claim(ClaimTypes.Country, "Palestine")
//     ]);
//
//     //Set the User Info / Claims Principal
//     ctx.User = new ClaimsPrincipal(identity);
//
//     return next.Invoke();
// });

#endregion

//Use Custom JWT Token Validation MiddleWare
app.UseMiddleware<TokenValidationMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(); // Enables serving files from wwwroot
app.UseHttpsRedirection();
app.MapControllers();

app.Run();