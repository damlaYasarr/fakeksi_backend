
using Autofac.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using WEBAPI.Data;
using WEBAPI.Services;
using WEBAPI.Utilities.Security.Encryption;
using WEBAPI.Utilities.Security.JWT;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IUserService, UserService>();


builder.Services.AddScoped<ITagEntryService, TagEntryService>();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenHelper, JwtHelper>();
builder.Services.AddDbContext<DataContext>();
builder.Services.AddSignalR();

builder.Services.AddRazorPages();


builder.Services.AddCors();



/*
            services.AddDependencyResolvers(new ICoreModule[]
            {
                new CoreModule(),
            });
 
 */
var tokenoptions = builder.Configuration.GetSection("TokenOptions").Get<WEBAPI.Utilities.Security.JWT.TokenOptions>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = tokenoptions.Issuer,
        ValidAudience = tokenoptions.Audience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenoptions.SecurityKey)
    };
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(builder => builder.WithOrigins("http://localhost:4200").AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    );
app.UseHttpsRedirection();
app.UseAuthentication();//first
app.UseAuthorization();
app.UseRouting();
app.MapControllers();

app.Run();
