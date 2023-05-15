using Binance.Net.Objects.Models.Spot;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Clients;
using WebApplication1.Models;

namespace WebApplication1.Contollers
{
    [ApiController]
    [Route("[controller]")]
    public class BinanceController : ControllerBase
    {
        private readonly ILogger<BinanceController> _logger;

        public BinanceController(ILogger<BinanceController> logger)
        {
            _logger = logger;
        }

        [HttpGet("Pair")]
        public Task<List<PairList>> Pair()
        {
            var client = new BinanceInfo();
            return client.CurrentPairPriceAsync();
        }
        
        [HttpGet("Balance")]
        public Task<List<CurrentBalance>> CurrentBalance()
        {
            return BinanceInfo.GetCurrentBalanceAsync();
        }
        [HttpGet("DepositHistory")]
        public async Task<List<DepositHistory>> DepositHistory(string apiKey, string secretKey)
        {
            var depositHistory = await BinanceInfo.GetDepositHistoryAsync(apiKey, secretKey);
            return depositHistory;
        }
        [HttpGet("WithdrawalHistory")]
        public async Task<List<WithdrawHistory>> WithdrawHistory(string apiKey, string secretKey)
        {
            var withdrawHistory = await BinanceInfo.GetWitdrawHistoryAsync(apiKey, secretKey);
            return withdrawHistory;
        }
        [HttpGet("DepositAdress")]
        public async Task<string> GetDepositAdressAsync(string asset, string network)
        {
            var adress = await BinanceInfo.GetDepositAdressAsync(asset, network);
            return adress;
        }
        [HttpPost("CreateOrder")]
        public async Task CreateOrder(string apiKey, string secretKey,string symbol, decimal quantity)
        {
            var order = await BinanceInfo.CreateOrderAsync(apiKey, secretKey, symbol, quantity);
        }
    }
}