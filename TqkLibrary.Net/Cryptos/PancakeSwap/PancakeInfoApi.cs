using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Cryptos.PancakeSwap
{
  public class PancakeInfoApi : BaseApi
  {
    /// <summary>
    /// Returns the token information, based on address.
    /// </summary>
    /// <param name="address"></param>
    /// <returns></returns>
    public Task<BaseResponse<TokenData>> Token(string address, CancellationToken cancellationToken = default)
      => RequestGet<BaseResponse<TokenData>>($"https://api.pancakeswap.info/api/v2/tokens/{address}", cancellationToken);

    /// <summary>
    /// Returns the tokens in the top ~1000 pairs on PancakeSwap, sorted by reserves.
    /// </summary>
    /// <returns></returns>
    public Task<BaseResponse<Dictionary<string, TokenData>>> Tokens(CancellationToken cancellationToken = default)
      => RequestGet<BaseResponse<Dictionary<string, TokenData>>>("https://api.pancakeswap.info/api/v2/tokens", cancellationToken);

    /// <summary>
    /// Returns data for the top ~1000 PancakeSwap pairs, sorted by reserves.
    /// </summary>
    /// <returns></returns>
    public Task<BaseResponse<Dictionary<string, TokenPairsData>>> PancakeSwapPairs(CancellationToken cancellationToken = default)
      => RequestGet<BaseResponse<Dictionary<string, TokenPairsData>>>("https://api.pancakeswap.info/api/v2/pairs", cancellationToken);
  }
}
