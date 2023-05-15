using Newtonsoft.Json;
using Binance.Net.Clients;
using Binance.Net.Enums;
using Binance.Net.Objects;
using Binance.Net.Objects.Models.Spot;
using CryptoExchange.Net.Objects;
using WebApplication1.Models;


namespace WebApplication1.Clients
{
    public class BinanceInfo
    {
        private HttpClient _httpClient;
        private static string _adress;
        private static string _secretKey;
        private static string _apiKey;
        string[] pairs = { "BTCUSDT", "ETHUSDT", "LTCUSDT" };

        public BinanceInfo()
        {
            _secretKey = Constants.secretKey;
            _adress = Constants.Adress;
            _apiKey = Constants.apiKey;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_adress);
        }

        public async Task<List<PairList>> CurrentPairPriceAsync()
        {
            var results = new List<PairList>();
            try
            {
                foreach (var pair in pairs)
                {
                    var responseString = await _httpClient.GetAsync($"api/v3/ticker/price?symbol={pair}");
                    responseString.EnsureSuccessStatusCode();
                    var content = responseString.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<PairList>(content);
                    if (result != null) results.Add(result);
                }
            }
            catch (HttpRequestException)
            {
                Console.WriteLine("error");
            }

            return results;
        }

        public static async Task<List<CurrentBalance>> GetCurrentBalanceAsync()
        {
            var balances = new List<CurrentBalance>();
            var options = new BinanceClientOptions()
            {
                ApiCredentials = new BinanceApiCredentials(_apiKey, _secretKey),
            };

            var client = new BinanceClient(options);

            var accountInfo = await client.SpotApi.Account.GetAccountInfoAsync();
            foreach (var balance in accountInfo.Data.Balances)
            {
                if (balance.Available > 0 || balance.Locked > 0)
                {
                    balances.Add(new CurrentBalance { Asset = balance.Asset, Total = balance.Total });
                }
            }

            return balances;
        }

        public static async Task<List<DepositHistory>> GetDepositHistoryAsync(string apiKey, string secretKey)
        {
            var depositHistory = new List<DepositHistory>();
            var options = new BinanceClientOptions()
            {
                ApiCredentials = new BinanceApiCredentials(apiKey, secretKey),
            };
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

        public static async Task<List<WithdrawHistory>> GetWitdrawHistoryAsync(string apiKey, string secretKey)
        {
            var withdrawHisoty = new List<WithdrawHistory>();
            var options = new BinanceClientOptions()
            {
                ApiCredentials = new BinanceApiCredentials(apiKey, secretKey),
            };
            var client = new BinanceClient(options);
            var accountInfo = await client.SpotApi.Account.GetWithdrawalHistoryAsync();
            var lastFiveWithdrawals = accountInfo.Data.Take(5);

            foreach (var withdrawal in lastFiveWithdrawals)
            {
                withdrawHisoty.Add(new WithdrawHistory()
                {
                    Adress = withdrawal.Address,
                    Status = withdrawal.Status,
                    Asset = withdrawal.Asset,
                    Amount = withdrawal.Quantity
                });
            }

            return withdrawHisoty;
        }

        public static async Task<string> GetDepositAdressAsync(string asset, string network)
        {
            var options = new BinanceClientOptions()
            {
                ApiCredentials = new BinanceApiCredentials(_apiKey, _secretKey)
            };
            var client = new BinanceClient(options);

            var depositAdress = await client.SpotApi.Account.GetDepositAddressAsync(asset, network);
            var adress = depositAdress.Data.Address;
            return adress;
        }

        public static async Task<WebCallResult<BinancePlacedOrder>> CreateOrderAsync(string apiKey, string secretKey, string symbol, decimal quantity)
        {
            var options = new BinanceClientOptions()
            {
                ApiCredentials = new BinanceApiCredentials(apiKey, secretKey)
            };
            var client = new BinanceClient(options);
            var order = await client.SpotApi.Trading.PlaceOrderAsync(symbol, OrderSide.Buy, SpotOrderType.Market, quantity);
            return order;
        }
    }
}