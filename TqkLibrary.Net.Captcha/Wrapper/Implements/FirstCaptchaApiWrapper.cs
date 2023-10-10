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
    public class FirstCaptchaApiWrapper : ICaptchaWrapper
    {
        readonly FirstCaptchaApi _firstCaptchaApi;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        public FirstCaptchaApiWrapper(string apiKey) : this(new FirstCaptchaApi(apiKey))
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstCaptchaApi"></param>
        public FirstCaptchaApiWrapper(FirstCaptchaApi firstCaptchaApi)
        {
            this._firstCaptchaApi = firstCaptchaApi;
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
        public async Task<ICaptchaTask<BasicCaptchaTaskResult>> CreateGoogleRecaptchaV2TaskAsync(string url, string siteKey, CancellationToken cancellationToken = default)
        {
            var task = await _firstCaptchaApi.ResolveRecaptchaV2Async(siteKey, url, IsInvisible, cancellationToken);
            return new RecaptchaV2Task(task);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsMath { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bitmapBuffer"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ICaptchaTask<BasicCaptchaTaskResult>> CreateImageCaptchaTaskAsync(byte[] bitmapBuffer, CancellationToken cancellationToken = default)
        {
            var task = await _firstCaptchaApi.ResolveImageTextAsync(bitmapBuffer, IsMath, cancellationToken);
            return new ImageCaptchaTask(task);
        }


        class RecaptchaV2Task : ICaptchaTask<BasicCaptchaTaskResult>
        {
            readonly FirstCaptchaApi.ITaskResponse<FirstCaptchaApi.TaskData> _task;
            public RecaptchaV2Task(FirstCaptchaApi.ITaskResponse<FirstCaptchaApi.TaskData> task)
            {
                this._task = task;
            }
            public async Task<BasicCaptchaTaskResult> GetTaskResultAsync(int delay = 2000, CancellationToken cancellationToken = default)
            {
                var res = await _task.GetTaskResultAsync(delay, cancellationToken);
                return new BasicCaptchaTaskResult()
                {
                    IsSuccess = res.Code == 0 && res.Status == FirstCaptchaApi.TaskResultStatus.SUCCESS,
                    ErrorMessage = res.Message,
                    Value = res.Data?.Token,
                };
            }
        }
        class ImageCaptchaTask : ICaptchaTask<BasicCaptchaTaskResult>
        {
            readonly FirstCaptchaApi.ITaskResponse<string> _task;
            public ImageCaptchaTask(FirstCaptchaApi.ITaskResponse<string> task)
            {
                this._task = task;
            }
            public async Task<BasicCaptchaTaskResult> GetTaskResultAsync(int delay = 2000, CancellationToken cancellationToken = default)
            {
                var res = await _task.GetTaskResultAsync(delay, cancellationToken);
                return new BasicCaptchaTaskResult()
                {
                    IsSuccess = res.Code == 0 && res.Status == FirstCaptchaApi.TaskResultStatus.SUCCESS,
                    ErrorMessage = res.Message,
                    Value = res.Data,
                };
            }
        }
    }
}
