using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Cryptos
{
  public class PancakeInfoApi : BaseApi
  {
    public PancakeInfoApi(CancellationToken cancellationToken = default): base(cancellationToken)
    {

    }
    /// <summary>
    /// Returns the token information, based on address.
    /// </summary>
    /// <param name="address"></param>
    /// <returns></returns>
    public Task<PancakeSwapBaseResponse<PancakeSwapTokenData>> Token(string address)
      => RequestGet<PancakeSwapBaseResponse<PancakeSwapTokenData>>($"https://api.pancakeswap.info/api/v2/tokens/{address}");

    /// <summary>
    /// Returns the tokens in the top ~1000 pairs on PancakeSwap, sorted by reserves.
    /// </summary>
    /// <returns></returns>
    public Task<PancakeSwapBaseResponse<Dictionary<string, PancakeSwapTokenData>>> Tokens()
      => RequestGet<PancakeSwapBaseResponse<Dictionary<string, PancakeSwapTokenData>>>("https://api.pancakeswap.info/api/v2/tokens");

    /// <summary>
    /// Returns data for the top ~1000 PancakeSwap pairs, sorted by reserves.
    /// </summary>
    /// <returns></returns>
    public Task<PancakeSwapBaseResponse<Dictionary<string, PancakeSwapTokenPairsData>>> PancakeSwapPairs()
      => RequestGet<PancakeSwapBaseResponse<Dictionary<string, PancakeSwapTokenPairsData>>>("https://api.pancakeswap.info/api/v2/pairs");
  }
}
