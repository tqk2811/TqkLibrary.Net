using System.Threading;
using System.Threading.Tasks;
using TqkLibrary.Net.Captcha.Wrapper.Classes;

namespace TqkLibrary.Net.Captcha.Wrapper.Interfaces
{
    public interface IRecaptchaV2TokenWrapper
    {
        Task<ICaptchaTask<CaptchaTaskTextResult>> CreateRecaptchaV2TokenTaskAsync(
            RecaptchaV2DataRequest recaptchaV2DataRequest,
            CancellationToken cancellationToken = default);
    }
}
