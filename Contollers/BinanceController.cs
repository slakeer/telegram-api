using Binance.Net.Enums;
using Binance.Net.Interfaces;
using Binance.Net.Objects.Models.Spot;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Clients;
using WebApplication1.Models;
using WebApplication1.MongoDB;

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
        public Task<List<PairList>> Pair(double chatId)
        {
            return BinanceInfo.CurrentPairPriceAsync(chatId);
        }
        
        [HttpGet("Balance")]
        public Task<List<CurrentBalance>> CurrentBalance(double chatId)
        {
            return BinanceInfo.GetCurrentBalanceAsync(chatId);
        }
        [HttpGet("deposit_history")]
        public async Task<List<DepositHistory>> DepositHistory(double chatId)
        {
            var depositHistory = await BinanceInfo.GetDepositHistoryAsync(chatId);
            return depositHistory;
        }
        [HttpGet("withdrawal_history")]
        public async Task<List<WithdrawHistory>> WithdrawHistory(double chatId)
        {
            var withdrawHistory = await BinanceInfo.GetWithdrawHistoryAsync(chatId);
            return withdrawHistory;
        }
        [HttpGet("deposit_adress")]
        public async Task<string> GetDepositAddressAsync(double chatId,string asset, string network)
        {
            var adress = await BinanceInfo.GetDepositAddressAsync(chatId,asset, network);
            return adress;
        } 
        [HttpGet("withdraw")]
        public async Task<IActionResult> Withdraw(double chatId, string address, string asset, decimal quantity)
        {
            var withdraw = await BinanceInfo.WithdrawAsync(chatId, address, asset, quantity);
            if (withdraw.Success)
            {
                return Ok(new { message = "Done" });
            }
            else
            {
                return BadRequest(new { error = $"Error: {withdraw.Error}" });
            }
        }
        [HttpPost("create_order_buy")]
        public async Task<Order> CreateOrderBuy(double chatId, string symbol, decimal quantity)
        {
            var order = await BinanceInfo.CreateOrderBuyAsync(chatId,symbol, quantity);
            return order;
        }
        [HttpPost("create_order_sell")]
        public async Task<Order> CreateOrderSell(double chatId, string symbol, decimal quantity)
        {
            var order = await BinanceInfo.CreateOrderBuyAsync(chatId,symbol, quantity);
            return order;
        }
        [HttpGet("price_reminder")]
        public async Task<ActionResult<PriceReminder>> PriceReminder(double chatId, string symbol)
        {
            try
            {
                var order = await BinanceInfo.PriceReminderAsync(chatId, symbol);
                return order;
            }
            catch (Exception ex)
            {
                return BadRequest("Произошла ошибка при получении данных о цене.");
            }
        }

        [HttpDelete("delete_user_data")]
        public async Task DeleteData(double chatId)
        {
            try
            {
                await BinanceInfo.DeleteUserData(chatId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        [HttpGet("get_klines")]
        public async Task<List<Klines>> GetKlines(double chatId, string symbol, KlineInterval Interval, DateTime? startTime, DateTime? endTime)
        {
            var result = await BinanceInfo.GetKlinesAsync(chatId,symbol, Interval, startTime, endTime);
            return result;
        }
        [HttpGet("/CryptoState/get_news")]
        public async Task<Article> GetNews()
        {
            Article randomArticle = await CryptoSlateNews.DisplayRandomArticle();
            return randomArticle;
        }
        [HttpGet("cancel_order")]
        public async Task<IActionResult> CancelOrder(double chatId, string Symbol)
        {
            try
            {
                await BinanceInfo.CancelOrderAsync(chatId, Symbol);
                return Ok("Order canceled successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest("Error canceling order: " + ex.Message);
            }
        }
    }
}