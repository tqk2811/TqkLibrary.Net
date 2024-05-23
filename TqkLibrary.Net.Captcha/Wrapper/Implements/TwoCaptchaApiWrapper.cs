using System;
using System.Threading;
using System.Threading.Tasks;
using static TqkLibrary.Net.Captcha.TwoCaptchaApi;

namespace TqkLibrary.Net.Captcha.Wrapper.Implements
{
    /// <summary>
    /// 
    /// </summary>
    public class TwoCaptchaApiWrapper : ICaptchaWrapper
    {
        readonly TwoCaptchaApi twoCaptchaApi;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        public TwoCaptchaApiWrapper(string apiKey) : this(new TwoCaptchaApi(apiKey))
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="twoCaptchaApi"></param>
        public TwoCaptchaApiWrapper(TwoCaptchaApi twoCaptchaApi)
        {
            this.twoCaptchaApi = twoCaptchaApi;
        }

        public int? SoftId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="siteKey"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ICaptchaTask<BasicCaptchaTaskResult>> CreateRecaptchaV2TaskAsync(
            RecaptchaV2DataRequest recaptchaV2DataRequest,
            CancellationToken cancellationToken = default)
        {
            if(recaptchaV2DataRequest is null)
                throw new ArgumentNullException(nameof(recaptchaV2DataRequest));

            TwoCaptchaApi.RecaptchaV2Request request = TwoCaptchaApi.RecaptchaV2Request.CloneFrom(recaptchaV2DataRequest);
            request.SoftId = SoftId;
            TwoCaptchaResponse twoCaptchaResponse = await twoCaptchaApi.RecaptchaV2Async(
                request,
                cancellationToken: cancellationToken);
            return new CaptchaTask(twoCaptchaApi, twoCaptchaResponse);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bitmapBuffer"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ICaptchaTask<BasicCaptchaTaskResult>> CreateImageCaptchaTaskAsync(
            byte[] bitmapBuffer,
            CancellationToken cancellationToken = default)
        {
            TwoCaptchaResponse twoCaptchaResponse = await twoCaptchaApi.ImageCaptchaAsync(bitmapBuffer, cancellationToken);
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
