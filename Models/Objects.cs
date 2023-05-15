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

        public PriceResponse()
        {
            Current = new List<CurrentBalance>();
            PairList = new List<PairList>();
            DepositHistories = new List<DepositHistory>();
            WithdrawHistories = new List<WithdrawHistory>();
        }
    }

    public class CurrentBalance
    {
        public string Asset { get; set; }
        public decimal Total { get; set; }
        
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
}
