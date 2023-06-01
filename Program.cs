
using CryptoExchange.Net.CommonObjects;
using WebApplication1;
using WebApplication1.Clients;
using WebApplication1.Models;
using WebApplication1.MongoDB;
using OpenApiInfo = Microsoft.OpenApi.Models.OpenApiInfo;

var builder = WebApplication.CreateBuilder(args);


void ConfigureServices(IServiceCollection services)
{

    services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API Title", Version = "v1" });

        options.DocInclusionPredicate((docName, description) => true);

        options.CustomSchemaIds(type => type.FullName);
    });
}

void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    // ...

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API Title v1");
    });
}

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.Run();