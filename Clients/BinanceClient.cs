using Binance.Net.Clients;
using Binance.Net.Enums;
using Binance.Net.Objects;
using Binance.Net.Objects.Models.Spot;
using CryptoExchange.Net.Objects;
using WebApplication1.Models;
using WebApplication1.MongoDB;

namespace WebApplication1.Clients;

public class BinanceInfo
{
    private string _apiKey;
    private string _secretKey;
    private static HttpClient _httpClient;
    static string[] pairs = { "BTCUSDT", "ETHUSDT", "LTCUSDT", "TRXUSDT", "XRPUSDT" };
    private static string API_KEY_MARKETCAP = "c75b8978-fdc2-4787-8fc4-46054794d584";


    private BinanceInfo(double chatId)
    {
        DataBase dataBase = new DataBase();
        dataBase.Main(chatId, out string apiKey, out string secretKey);
        _apiKey = apiKey;
        _secretKey = secretKey;
    }

    public static async Task<List<PairList>> CurrentPairPriceAsync(double chatId)
    {
        var results = new List<PairList>();
        var binanceInfo = new BinanceInfo(chatId);
        var options = new BinanceClientOptions()
        {
            ApiCredentials = new BinanceApiCredentials(binanceInfo._apiKey, binanceInfo._secretKey),
        };

        var client = new BinanceClient(options);
        try
        {
            foreach (var pair in pairs)
            {
                var ticker = await client.SpotApi.ExchangeData.GetTickerAsync(pair);
                if (true)
                {
                    var pairList = new PairList()
                    {
                        Symbol = ticker.Data.Symbol,
                        Price = ticker.Data.LastPrice
                    };
                    results.Add(pairList);
                }
            }
        }
        catch (HttpRequestException)
        {
            Console.WriteLine("error");
        }

        return results;
    }

    public static async Task<List<CurrentBalance>> GetCurrentBalanceAsync(double chatId)
    {
        var binanceInfo = new BinanceInfo(chatId);
        var balances = new List<CurrentBalance>();
        var options = new BinanceClientOptions()
        {
            ApiCredentials = new BinanceApiCredentials(binanceInfo._apiKey, binanceInfo._secretKey),
        };

        var client = new BinanceClient(options);

        var accountInfo = await client.SpotApi.Account.GetAccountInfoAsync();
        foreach (var balance in accountInfo.Data.Balances)
        {
            if (balance.Available > 0 || balance.Locked > 0)
            {
                var convertToUsd =
                    await client.SpotApi.ExchangeData.GetCurrentAvgPriceAsync($"{balance.Asset}USDT");
                var currencyUsdPrice = convertToUsd.Data.Price;
                var usdBalance = currencyUsdPrice * balance.Total;
                balances.Add(new CurrentBalance
                    { Asset = balance.Asset, Total = balance.Total, UsdPrice = usdBalance });
            }
        }

        return balances;
    }

    public static async Task<List<DepositHistory>> GetDepositHistoryAsync(double chatId)
    {
        var binanceInfo = new BinanceInfo(chatId);
        var depositHistory = new List<DepositHistory>();
        var options = new BinanceClientOptions()
        {
            ApiCredentials = new BinanceApiCredentials(binanceInfo._apiKey, binanceInfo._secretKey),
        };
        Console.WriteLine(binanceInfo._apiKey);
        Console.WriteLine(binanceInfo._secretKey);

        var client = new BinanceClient(options);
        var accountInfo = await client.SpotApi.Account.GetDepositHistoryAsync();
        var lastFiveDeposits = accountInfo.Data.Take(5);

        foreach (var deposit in lastFiveDeposits)
        {
            depositHistory.Add(new DepositHistory()
            {
                Adress = deposit.Address,
                Status = deposit.Status,
                Asset = deposit.Asset,
                Amount = deposit.Quantity
            });
        }

        return depositHistory;
    }

    public static async Task<List<WithdrawHistory>> GetWithdrawHistoryAsync(double chatId)
    {
        var binanceInfo = new BinanceInfo(chatId);
        var withdrawHistory = new List<WithdrawHistory>();
        var options = new BinanceClientOptions()
        {
            ApiCredentials = new BinanceApiCredentials(binanceInfo._apiKey, binanceInfo._secretKey),
        };
        var client = new BinanceClient(options);
        var accountInfo = await client.SpotApi.Account.GetWithdrawalHistoryAsync();
        var lastFiveWithdrawals = accountInfo.Data.Take(5);

        foreach (var withdrawal in lastFiveWithdrawals)
        {
            withdrawHistory.Add(new WithdrawHistory()
            {
                Adress = withdrawal.Address,
                Status = withdrawal.Status,
                Asset = withdrawal.Asset,
                Amount = withdrawal.Quantity
            });
        }

        return withdrawHistory;
    }

    public static async Task<string> GetDepositAddressAsync(double chatId, string asset, string network)
    {
        var binanceInfo = new BinanceInfo(chatId);
        var options = new BinanceClientOptions()
        {
            ApiCredentials = new BinanceApiCredentials(binanceInfo._apiKey, binanceInfo._secretKey),
        };
        var client = new BinanceClient(options);

        var depositAddress = await client.SpotApi.Account.GetDepositAddressAsync(asset, network);
        var address = depositAddress.Data.Address;
        return address;
    }

    public static async Task<WebCallResult<BinanceWithdrawalPlaced>> WithdrawAsync(double chatId, string address, string asset, decimal quantity)
    {
        var binanceInfo = new BinanceInfo(chatId);
        var options = new BinanceClientOptions()
        {
            ApiCredentials = new BinanceApiCredentials(binanceInfo._apiKey, binanceInfo._secretKey)
        };
        var client = new BinanceClient(options);
        var order = await client.SpotApi.Account.WithdrawAsync(asset, address, quantity);
        return order;
    }

    public static async Task<Order> CreateOrderBuyAsync(double chatId, string symbol, decimal quantity)
    {
        var binanceInfo = new BinanceInfo(chatId);
        var options = new BinanceClientOptions()
        {
            ApiCredentials = new BinanceApiCredentials(binanceInfo._apiKey, binanceInfo._secretKey),
        };
        var client = new BinanceClient(options);

        var order = await client.SpotApi.Trading.PlaceOrderAsync(symbol, OrderSide.Buy, SpotOrderType.Market, quantity);
        var result = new Order
        {
            Symbol = symbol,
            Quantity = quantity,
            Id = order.Data.Id,
            Status = order.Data.Status,
            Side = order.Data.Side
        };
        if (order.Success)
        {
            Console.WriteLine("Order placed successfully.");
        }
        else
        {
            Console.WriteLine("Error placing order: " + order.Error.Message);
        }


        return result;
    }

    public static async Task<Order> CreateOrderSellAsync(double chatId, string symbol, decimal quantity)
    {
        var binanceInfo = new BinanceInfo(chatId);
        var options = new BinanceClientOptions()
        {
            ApiCredentials = new BinanceApiCredentials(binanceInfo._apiKey, binanceInfo._secretKey),
           
        };
        var client = new BinanceClient(options);
        var order = await client.SpotApi.Trading.PlaceOrderAsync(symbol, OrderSide.Sell, SpotOrderType.Market, quantity);
        var result = new Order
        {
            Symbol = symbol,
            Quantity = quantity,
            Side = order.Data.Side,
            Id = order.Data.Id,
            Status = order.Data.Status
        };
        if (order.Success)
        {
            Console.WriteLine($"Successfully sold {quantity} {symbol} by price {order.Data.Price}.");
        }
        else
        {
            Console.WriteLine($"Failed to sell coin: {order.Error.Message}");
        }

        return result;
    }

    public static async Task CancelOrderAsync(double chatId, string symbol)
    {
        var binanceInfo = new BinanceInfo(chatId);
        var options = new BinanceClientOptions()
        {
            ApiCredentials = new BinanceApiCredentials(binanceInfo._apiKey, binanceInfo._secretKey),

        };
        var client = new BinanceClient(options);
        var cancelResult = await client.SpotApi.Trading.CancelAllOrdersAsync(symbol);

        if (cancelResult.Success)
        {
            Console.WriteLine("Order canceled successfully.");
        }
        else
        {
            Console.WriteLine("Error canceling order: " + cancelResult.Error.Message);
        }
    }

    public static async Task DeleteUserData(double chatId)
    {
        try
        {
            DataBase dataBase = new DataBase();
            await dataBase.DeleteUserAsync(chatId);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while deleting user data: " + ex.Message);
        }
    }
    public static async Task<PriceReminder> PriceReminderAsync(double chatId, string symbol)
    {
        var binanceInfo = new BinanceInfo(chatId);
        var options = new BinanceClientOptions()
        {
            ApiCredentials = new BinanceApiCredentials(binanceInfo._apiKey, binanceInfo._secretKey)
        };
        var client = new BinanceClient(options);
        var priceResult = await client.SpotApi.ExchangeData.GetPriceAsync(symbol);
        var price = new PriceReminder()
        {
            Symbol = symbol,
            Price = priceResult.Data.Price
        };
        if (priceResult.Success)
        {
            Console.WriteLine($"Price reminder set {priceResult.Data.Symbol} {priceResult.Data.Price}");
        }
        else
        {
            Console.WriteLine("Error set reminder: " + priceResult.Error.Message);
        }

        return price;
    }
    public static async Task<List<Klines>> GetKlinesAsync(double chatId, string symbol, KlineInterval Interval, DateTime? startTime, DateTime? endTime)
    {
        var binanceInfo = new BinanceInfo(chatId);
        var result = new List<Klines>();
        var options = new BinanceClientOptions()
        {
            ApiCredentials = new BinanceApiCredentials(binanceInfo._apiKey, binanceInfo._secretKey)
        };
        var client = new BinanceClient(options);
        var data = await client.SpotApi.ExchangeData.GetKlinesAsync(symbol, Interval, startTime, endTime);
    
        if (data.Success)
        {
            var klines = data.Data;
        
            foreach (var kline in klines)
            {
                var klineResult = new Klines
                {
                    openPrice = kline.OpenPrice,
                    highPrice = kline.HighPrice,
                    lowPrice = kline.LowPrice,
                    closePrice = kline.ClosePrice,
                    openTime = kline.OpenTime,
                    clostTime = kline.CloseTime,
                    volume = kline.Volume
                };

                result.Add(klineResult);
            }
        }
        else
        {
            Console.WriteLine($"Error: {data.Error}");
        }
    
        return result;
    }
}