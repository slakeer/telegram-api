using System.Reflection.Metadata;
using CryptoExchange.Net.CommonObjects;
using WebApplication1;
using WebApplication1.Clients;
using WebApplication1.Models;
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
    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API Title v1"); });
}

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.Run();

var pairPrice = new BinanceInfo();

var results = await pairPrice.CurrentPairPriceAsync();
foreach (var price in results)
{
    Console.WriteLine($"{price.Symbol} / {price.Price}");
}

var balances = await BinanceInfo.GetCurrentBalanceAsync();
foreach (var price in balances)
{
    Console.WriteLine($"{price.Asset} / {price.Total}");
}

var depositHistory = await BinanceInfo.GetDepositHistoryAsync(Constants.apiKey, Constants.secretKey);
foreach (var data in depositHistory)
{
    Console.WriteLine($" Status: {data.Status}   Coin:{data.Asset}   Amount:{data.Amount}   Adress:{data.Adress} ");
}

var withdrawalHistory = await BinanceInfo.GetWitdrawHistoryAsync(Constants.apiKey, Constants.secretKey);
foreach (var data in withdrawalHistory)
{
    Console.WriteLine($" Status: {data.Status}   Coin:{data.Asset}   Amount:{data.Amount}   Adress:{data.Adress} ");
}

var depositAdress = await BinanceInfo.GetDepositAdressAsync("TRX", "TRX");
Console.WriteLine(depositAdress);


Console.ReadKey();