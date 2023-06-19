using ApiCatalogo.Extensions;
using ApiCatalogo.Repository;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddEndpointsApiExplorer();
builder.AddApiSwagger();
builder.AddPersistence();
builder.Services.AddScoped<IUnitofWork, UnitOfWork>();
builder.AddMapper();
builder.Services.AddCors();
builder.AddIdentity();
builder.AddToken();
builder.AddVersion();

var app = builder.Build();

var environment = app.Environment;
app.UseExceptionHandling(environment);

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();