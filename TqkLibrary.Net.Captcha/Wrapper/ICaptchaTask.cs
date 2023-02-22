using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Captcha.Wrapper
{
    public interface ICaptchaTask<T>
    {
        Task<T> GetTaskResultAsync(int delay = 2000, CancellationToken cancellationToken = default);
    }
}
