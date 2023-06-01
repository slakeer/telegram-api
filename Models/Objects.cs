using Binance.Net.Enums;
using CryptoExchange.Net.CommonObjects;
using Newtonsoft.Json;

namespace WebApplication1.Models
{
    public class PriceResponse
    {
        public List<CurrentBalance> Current { get; set; }
        public List<PairList> PairList { get; set; }
        public List<DepositHistory> DepositHistories { get; set; }
        public List<WithdrawHistory> WithdrawHistories { get; set; }
        public Withdraw Withdraw { get; set; }
        public Charts Charts { get; set; }
        public Order order { get; set; }
        public PriceReminder price { get; set; }
        public List<Klines> Klines { get; set; }

        public PriceResponse()
        {
            Current = new List<CurrentBalance>();
            PairList = new List<PairList>();
            DepositHistories = new List<DepositHistory>();
            WithdrawHistories = new List<WithdrawHistory>();
            Withdraw = new Withdraw();
            order = new Order();
            price = new PriceReminder();
            Klines = new List<Klines>();
        }
    }

    public class CurrentBalance
    {
        public string Asset { get; set; }
        public decimal Total { get; set; }
        public decimal UsdPrice { get; set; }
    }
    public class Klines
    {
        public decimal openPrice { get; set; }
        public decimal highPrice { get; set; }
        public decimal lowPrice { get; set; }
        public decimal closePrice { get; set; }
        public DateTime openTime { get; set; }
        public DateTime clostTime { get; set; }
        public decimal volume { get; set; }
    }
    public class Article
    {
        public string ArticleId { get; }
        public string Title { get; }
        public string Link { get; }
        public Article(string articleId, string title, string link)
        {
            ArticleId = articleId;
            Title = title;
            Link = link;
        }
        
    }

    public class Withdraw
    {
        public string Asset { get; set; }
        public string Adress { get; set; }
        public decimal Amount { get; set; }
        public string Network { get; set; }
    }
    public class OrderCancellationRequest
    {
        public double ChatId { get; set; }
        public string Symbol { get; set; }
    }
    public class PriceReminder
    {
        public string Symbol { get; set; }
        public decimal Price { get; set; }
    }

    public class Order
    {
        public string? Symbol { get; set; }
        public decimal Quantity { get; set; }
        public long Id { get; set; }
        public OrderStatus Status { get; set; }
        public OrderSide Side { get; set; }
    }
    public class OrderRequest
    {
        public double ChatId { get; set; }
        public string Symbol { get; set; }
        public decimal Quantity { get; set; }
    }
    public class DepositHistory
    {
        public DepositStatus Status { get; set; }
        public string Adress { get; set; }
        public string Asset { get; set; }
        public decimal Amount { get; set; }
    }

    public class WithdrawHistory
    {
        public WithdrawalStatus Status { get; set; }
        public string Adress { get; set; }
        public string Asset { get; set; }
        public decimal Amount { get; set; }
    }

    public class PairList
    {
        public string Symbol { get; set; }
        public decimal Price { get; set; }
    }
    public class Charts
    {
        public string symbol { get; set; }
        public KlineInterval interval { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public int limit { get; set; }
    }
}