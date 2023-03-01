using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Captcha.Wrapper.Implements
{
    public class AnyCaptchaApiWrapper : ICaptchaWrapper
    {
        readonly AnyCaptchaApi anyCaptchaApi;
        public AnyCaptchaApiWrapper(string apiKey) : this(new AnyCaptchaApi(apiKey))
        {

        }
        public AnyCaptchaApiWrapper(AnyCaptchaApi anyCaptchaApi)
        {
            this.anyCaptchaApi = anyCaptchaApi;
        }

        public bool IsRecaptchaV2Invisible { get; set; } = false;


        public async Task<ICaptchaTask<BasicCaptchaTaskResult>> CreateGoogleRecaptchaV2TaskAsync(string url, string siteKey, CancellationToken cancellationToken = default)
        {
            IAnyCaptchaTaskResponse anyCaptchaTaskResponse = await anyCaptchaApi
                .RecaptchaV2TaskProxylessAsync(url, siteKey, IsRecaptchaV2Invisible, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
            return new CaptchaTask(anyCaptchaTaskResponse, s => s.GRecaptchaResponse);
        }
        public async Task<ICaptchaTask<BasicCaptchaTaskResult>> CreateImageCaptchaTaskAsync(byte[] bitmapBuffer, CancellationToken cancellationToken = default)
        {
            IAnyCaptchaTaskResponse anyCaptchaTaskResponse = await anyCaptchaApi
                .ImageToTextAsync(bitmapBuffer, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
            return new CaptchaTask(anyCaptchaTaskResponse, s => s.Text);
        }


        class CaptchaTask : ICaptchaTask<BasicCaptchaTaskResult>
        {
            readonly IAnyCaptchaTaskResponse anyCaptchaTaskResponse;
            readonly Expression<Func<GetTaskResultResponseSolution, string>> expression;
            public CaptchaTask(
                IAnyCaptchaTaskResponse anyCaptchaTaskResponse,
                Expression<Func<GetTaskResultResponseSolution, string>> expression)
            {
                this.anyCaptchaTaskResponse = anyCaptchaTaskResponse;
                this.expression = expression;
            }


            public async Task<BasicCaptchaTaskResult> GetTaskResultAsync(int delay = 2000, CancellationToken cancellationToken = default)
            {
                GetTaskResultResponse getTaskResultResponse = await anyCaptchaTaskResponse
                    .WaitForResultAsync(delay, cancellationToken)
                    .ConfigureAwait(false);

                if (getTaskResultResponse.ErrorId == 0)
                {
                    Func<GetTaskResultResponseSolution, string> func = expression.Compile();
                    return new BasicCaptchaTaskResult()
                    {
                        IsSuccess = true,
                        Value = func.Invoke(getTaskResultResponse.Solution),
                    };
                }
                else
                {
                    return new BasicCaptchaTaskResult()
                    {
                        IsSuccess = false,
                        ErrorMessage = getTaskResultResponse.ErrorDescription,
                    };
                }
            }
        }
    }
}
