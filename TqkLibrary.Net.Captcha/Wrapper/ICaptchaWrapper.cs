using System;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Captcha.Wrapper
{
    public interface ICaptchaWrapper
    {
        Task<ICaptchaTask<BasicCaptchaTaskResult>> CreateImageCaptchaTaskAsync(
            byte[] bitmapBuffer,
            CancellationToken cancellationToken = default);

        Task<ICaptchaTask<BasicCaptchaTaskResult>> CreateRecaptchaV2TaskAsync(
            RecaptchaV2DataRequest recaptchaV2DataRequest,
            CancellationToken cancellationToken = default);
    }
}
