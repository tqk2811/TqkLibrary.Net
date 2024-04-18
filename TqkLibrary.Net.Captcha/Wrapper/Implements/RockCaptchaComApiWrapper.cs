using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Captcha.Wrapper.Implements
{
    /// <summary>
    /// 
    /// </summary>
    public class RockCaptchaComApiWrapper : ICaptchaWrapper
    {
        readonly RockCaptchaComApi _rockCaptchaComApi;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        public RockCaptchaComApiWrapper(string apiKey)
        {
            _rockCaptchaComApi = new RockCaptchaComApi(apiKey);
        }
        /// <summary>
        /// 
        /// </summary>
        public RockCaptchaComApiWrapper(RockCaptchaComApi rockCaptchaComApi)
        {
            _rockCaptchaComApi = rockCaptchaComApi ?? throw new ArgumentNullException(nameof(rockCaptchaComApi));
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsInvisible { get; set; } = false;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="siteKey"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ICaptchaTask<BasicCaptchaTaskResult>> CreateGoogleRecaptchaV2TaskAsync(string url, string siteKey, CancellationToken cancellationToken = default)
        {
            var task = await _rockCaptchaComApi.CreateTaskRecaptchaV2Async(siteKey, url, IsInvisible, null, null, cancellationToken);
            return new RecaptchaV2Task(_rockCaptchaComApi, task);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="bitmapBuffer"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ICaptchaTask<BasicCaptchaTaskResult>> CreateImageCaptchaTaskAsync(byte[] bitmapBuffer, CancellationToken cancellationToken = default)
        {
            var task = await _rockCaptchaComApi.CreateTaskImageToTextAsync(bitmapBuffer, cancellationToken);
            return new ImageToTextTask(_rockCaptchaComApi, task);
        }


        abstract class CaptchaTask : ICaptchaTask<BasicCaptchaTaskResult>
        {
            protected readonly RockCaptchaComApi _rockCaptchaComApi;
            public CaptchaTask(RockCaptchaComApi rockCaptchaComApi)
            {
                this._rockCaptchaComApi = rockCaptchaComApi ?? throw new ArgumentNullException(nameof(rockCaptchaComApi));
            }
            public abstract Task<BasicCaptchaTaskResult> GetTaskResultAsync(int delay = 2000, CancellationToken cancellationToken = default);
        }
        class RecaptchaV2Task : CaptchaTask
        {
            readonly RockCaptchaComApi.CreateTaskResponse<RockCaptchaComApi.TaskDataResult> _createTaskResponse;
            public RecaptchaV2Task(
                RockCaptchaComApi rockCaptchaComApi,
                RockCaptchaComApi.CreateTaskResponse<RockCaptchaComApi.TaskDataResult> createTaskResponse

                ) : base(rockCaptchaComApi)
            {
                _createTaskResponse = createTaskResponse ?? throw new ArgumentNullException(nameof(createTaskResponse));
            }
            public override async Task<BasicCaptchaTaskResult> GetTaskResultAsync(int delay = 2000, CancellationToken cancellationToken = default)
            {
                var result = await _rockCaptchaComApi.WaitUntilResultAsync(_createTaskResponse, cancellationToken: cancellationToken);

                return new BasicCaptchaTaskResult()
                {
                    IsSuccess = result.Status == RockCaptchaComApi.Status.SUCCESS,
                    Value = result.Data?.Token!,
                    ErrorMessage = result.Message!,
                };
            }
        }
        class ImageToTextTask : CaptchaTask
        {
            readonly RockCaptchaComApi.CreateTaskResponse<string> _createTaskResponse;
            public ImageToTextTask(
                RockCaptchaComApi rockCaptchaComApi,
                RockCaptchaComApi.CreateTaskResponse<string> createTaskResponse
                ) : base(rockCaptchaComApi)
            {
                _createTaskResponse = createTaskResponse ?? throw new ArgumentNullException(nameof(createTaskResponse));
            }

            public override async Task<BasicCaptchaTaskResult> GetTaskResultAsync(int delay = 2000, CancellationToken cancellationToken = default)
            {
                var result = await _rockCaptchaComApi.WaitUntilResultAsync(_createTaskResponse, cancellationToken: cancellationToken);

                return new BasicCaptchaTaskResult()
                {
                    IsSuccess = result.Status == RockCaptchaComApi.Status.SUCCESS,
                    Value = result.Data,
                    ErrorMessage = result.Message!,
                };
            }
        }
    }
}
