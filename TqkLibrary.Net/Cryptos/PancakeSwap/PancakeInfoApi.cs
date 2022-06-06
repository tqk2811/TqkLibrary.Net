using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Cryptos
{
    /// <summary>
    /// 
    /// </summary>
    public class PancakeInfoApi : BaseApi
    {
        const string EndPoint = "https://api.pancakeswap.info/api/v2";
        /// <summary>
        /// 
        /// </summary>
        public PancakeInfoApi() : base()
        {

        }
        /// <summary>
        /// Returns the token information, based on address.
        /// </summary>
        /// <returns></returns>
        public Task<PancakeSwapBaseResponse<PancakeSwapTokenData>> Token(string address, CancellationToken cancellationToken = default)
            => Build()
            .WithUrlGet(new UriBuilder(EndPoint, "tokens", address))
            .ExecuteAsync<PancakeSwapBaseResponse<PancakeSwapTokenData>>(cancellationToken);

        /// <summary>
        /// Returns the tokens in the top ~1000 pairs on PancakeSwap, sorted by reserves.
        /// </summary>
        /// <returns></returns>
        public Task<PancakeSwapBaseResponse<Dictionary<string, PancakeSwapTokenData>>> Tokens(CancellationToken cancellationToken = default)
            => Build()
            .WithUrlGet(new UriBuilder(EndPoint, "tokens"))
            .ExecuteAsync<PancakeSwapBaseResponse<Dictionary<string, PancakeSwapTokenData>>>(cancellationToken);

        /// <summary>
        /// Returns data for the top ~1000 PancakeSwap pairs, sorted by reserves.
        /// </summary>
        /// <returns></returns>
        public Task<PancakeSwapBaseResponse<Dictionary<string, PancakeSwapTokenPairsData>>> PancakeSwapPairs(CancellationToken cancellationToken = default)
            => Build()
            .WithUrlGet(new UriBuilder(EndPoint, "pairs"))
            .ExecuteAsync<PancakeSwapBaseResponse<Dictionary<string, PancakeSwapTokenPairsData>>>(cancellationToken);
    }
}
