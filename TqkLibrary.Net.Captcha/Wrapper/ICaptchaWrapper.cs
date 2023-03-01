using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Captcha.Wrapper
{
    public interface ICaptchaWrapper
    {
        Task<ICaptchaTask<BasicCaptchaTaskResult>> CreateImageCaptchaTaskAsync(
            byte[] bitmapBuffer,
            CancellationToken cancellationToken = default);

        Task<ICaptchaTask<BasicCaptchaTaskResult>> CreateGoogleRecaptchaV2TaskAsync(
            string url,
            string siteKey,
            CancellationToken cancellationToken = default);
    }
}
