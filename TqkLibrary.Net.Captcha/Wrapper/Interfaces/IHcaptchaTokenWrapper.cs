using System.Threading;
using System.Threading.Tasks;
using TqkLibrary.Net.Captcha.Wrapper.Classes;

namespace TqkLibrary.Net.Captcha.Wrapper.Interfaces
{
    public interface IHcaptchaTokenWrapper
    {
        Task<ICaptchaTask<CaptchaTaskTextResult>> CreateHcaptchaTokenTaskAsync(
            HcaptchaDataRequest request,
            CancellationToken cancellationToken = default
            );
    }
}
