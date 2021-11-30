using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.OpenApi.Models;
using MyCloud.NoSqlDatabaseAdminService.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton(new Database());
builder.Services.AddRouting();
// required to parse JsonPatchDocument
builder.Services.AddControllers().AddNewtonsoftJson().AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())); ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1.0",
        Title = "MyCloud - NoSql Database Admin Service",
        Description = "The MyCloud - NoSql Database Admin Service provides a database service for modern app development. " +
                      "Enjoy elastic scalability while automating time‐consuming database administration tasks.",
        Contact = new OpenApiContact
        {
            Name = "Robin Müller",
            Email = "romuit04@hs-esslingen.de",
            Url = new Uri("https://github.com/RobinTTY/MyCloud")
        },
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://github.com/RobinTTY/MyCloud/blob/master/LICENSE")
        }
    });

    // Include Descriptions from XML comments
    var docsFilePath = Path.Combine(AppContext.BaseDirectory, "MyCloud.NoSqlDatabaseAdminService.xml");
    options.IncludeXmlComments(docsFilePath);
    options.EnableAnnotations();
});

builder.Services.AddApiVersioning(config =>
{
    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.AssumeDefaultVersionWhenUnspecified = true;
    config.ReportApiVersions = true;
    config.ApiVersionReader = new HeaderApiVersionReader("api-version");
});

var app = builder.Build();

// populate initial data
var db = app.Services.GetService<Database>();
db?.PopulateWithDemoData();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
