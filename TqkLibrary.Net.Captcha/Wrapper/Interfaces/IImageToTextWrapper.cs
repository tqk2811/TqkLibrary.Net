using System;
using System.Threading;
using System.Threading.Tasks;
using TqkLibrary.Net.Captcha.Wrapper.Classes;

namespace TqkLibrary.Net.Captcha.Wrapper.Interfaces
{
    public interface IImageToTextWrapper
    {
        Task<ICaptchaTask<CaptchaTaskTextResult>> CreateImageToTextTaskAsync(
            byte[] bitmapBuffer,
            CancellationToken cancellationToken = default);

    }
}
