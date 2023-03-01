namespace TqkLibrary.Net.Cryptos
{
    public class PancakeSwapBaseResponse<T>
    {
        public long updated_at { get; set; }
        public T data { get; set; }
    }
}
