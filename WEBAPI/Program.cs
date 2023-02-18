using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using WEBAPI.Data;
using WEBAPI.Services;
using WEBAPI.Utilities.IOC;
using WEBAPI.Utilities.Security.Encryption;
using WEBAPI.Utilities.Security.JWT;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITagEntryService, TagEntryService>();
builder.Services.AddScoped<IAuthService, AuthManager>();
builder.Services.AddDbContext<DataContext>();
builder.Services.AddSignalR();

var app = builder.Build();
builder.Services.AddCors();
//var tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();


//research : builder log detail

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

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();

app.Run();
