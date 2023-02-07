using WEBAPI.Data;
using WEBAPI.Services;

var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITagEntryService, TagEntryService>();
builder.Services.AddDbContext<DataContext>();
builder.Services.AddSignalR();
var app = builder.Build();
builder.Services.AddCors();

app.UseCors(builder => builder.WithOrigins("https://localhost:7095/swagger/index.html").AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());


//research : builder log detail

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(MyAllowSpecificOrigins);
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
