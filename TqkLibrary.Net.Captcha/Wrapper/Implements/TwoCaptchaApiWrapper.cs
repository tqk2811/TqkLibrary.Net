using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Captcha.Wrapper.Implements
{
    public class TwoCaptchaApiWrapper : ICaptchaWrapper
    {
        readonly TwoCaptchaApi twoCaptchaApi;
        public TwoCaptchaApiWrapper(string apiKey) : this(new TwoCaptchaApi(apiKey))
        {

        }
        public TwoCaptchaApiWrapper(TwoCaptchaApi twoCaptchaApi)
        {
            this.twoCaptchaApi = twoCaptchaApi;
        }



        public async Task<ICaptchaTask<BasicCaptchaTaskResult>> CreateGoogleRecaptchaV2TaskAsync(
            string url,
            string siteKey,
            CancellationToken cancellationToken = default)
        {
            TwoCaptchaResponse twoCaptchaResponse = await twoCaptchaApi.RecaptchaV2(
                siteKey,
                url,
                cancellationToken: cancellationToken);
            return new CaptchaTask(twoCaptchaApi, twoCaptchaResponse);
        }

        public async Task<ICaptchaTask<BasicCaptchaTaskResult>> CreateImageCaptchaTaskAsync(
            byte[] bitmapBuffer,
            CancellationToken cancellationToken = default)
        {
            TwoCaptchaResponse twoCaptchaResponse = await twoCaptchaApi.Nomal(bitmapBuffer, cancellationToken);
            return new CaptchaTask(twoCaptchaApi, twoCaptchaResponse);
        }

        class CaptchaTask : ICaptchaTask<BasicCaptchaTaskResult>
        {
            readonly TwoCaptchaApi twoCaptchaApi;
            readonly TwoCaptchaResponse twoCaptchaResponse;
            public CaptchaTask(TwoCaptchaApi twoCaptchaApi, TwoCaptchaResponse twoCaptchaResponse)
            {
                this.twoCaptchaApi = twoCaptchaApi;
                this.twoCaptchaResponse = twoCaptchaResponse;
            }

            public async Task<BasicCaptchaTaskResult> GetTaskResultAsync(int delay = 2000, CancellationToken cancellationToken = default)
            {
                var response = await twoCaptchaApi
                    .WaitResponseJsonCompleted(twoCaptchaResponse.Request, delay, cancellationToken)
                    .ConfigureAwait(false);
                if (response.CheckState() == TwoCaptchaState.Success)
                {
                    return new BasicCaptchaTaskResult()
                    {
                        IsSuccess = true,
                        Value = response.Request
                    };
                }
                else
                {
                    return new BasicCaptchaTaskResult()
                    {
                        IsSuccess = false,
                        Value = response.Request,
                        ErrorMessage = response.Request,
                    };
                }
            }
        }
    }
}
