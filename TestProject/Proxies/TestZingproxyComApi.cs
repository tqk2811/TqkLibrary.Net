
using TqkLibrary.Net;
using TqkLibrary.Net.Proxy.Services;

namespace TestProject.Proxies
{
    [TestClass]
    public class TestZingproxyComApi
    {
        public static IEnumerable<object[]> Datas
        {
            get
            {
                yield return new object[]
                {
                    "eyJhbGciOiJIU",
                };
            }
        }
        [TestMethod(), DynamicData(nameof(Datas))]
        public async Task Test(string accessToken)
        {
            using SocketsHttpHandler socketsHttpHandler = new SocketsHttpHandler();
            socketsHttpHandler.DisableFindIpV6();
            using ZingproxyComApi zingproxyComApi = new(accessToken, socketsHttpHandler, false);
            var list = await zingproxyComApi.DanCuVietNam.ListAsync(ZingproxyComApi.RunningType.Running);
            var changed = await zingproxyComApi.DanCuVietNam.GetIpAsync(list.Proxies!.First());
            var proxy = await zingproxyComApi.GetProxy(list.Proxies!.First()!);
        }
    }
}
