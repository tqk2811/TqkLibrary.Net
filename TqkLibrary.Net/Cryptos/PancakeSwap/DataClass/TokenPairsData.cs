namespace TqkLibrary.Net.Cryptos.PancakeSwap
{
  public class TokenPairsData
  {
    public string pair_address { get; set; }

    public string base_name { get; set; }
    public string base_symbol { get; set; }
    public string base_address { get; set; }
    public string base_volume { get; set; }

    public string quote_name { get; set; }
    public string quote_symbol { get; set; }
    public string quote_address { get; set; }
    public string quote_volume { get; set; }

    public string price { get; set; }
   
    public string liquidity { get; set; }
    public string liquidity_BNB { get; set; }
  }
}
