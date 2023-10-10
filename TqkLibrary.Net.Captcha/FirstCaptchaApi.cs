using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Captcha
{
    /// <summary>
    /// 
    /// </summary>
    public class FirstCaptchaApi : BaseApi
    {
        const string EndPoint = "https://api.1stcaptcha.com/";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        public FirstCaptchaApi(string apiKey) : base(apiKey)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageBuff"></param>
        /// <param name="isMath"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ITaskResponse<string>> ResolveImageTextAsync(byte[] imageBuff, bool isMath = false, CancellationToken cancellationToken = default)
        {
            var result = await Build()
                .WithUrlPostJson(new UrlBuilder(EndPoint, "Recognition"),
                new
                {
                    Apikey = ApiKey,
                    Type = "imagetotext",
                    Image = Convert.ToBase64String(imageBuff),
                    Math = isMath
                })
                .ExecuteAsync<TaskResponse<string>>(cancellationToken);
            result.FirstCaptchaApi = this;
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteKey"></param>
        /// <param name="url"></param>
        /// <param name="invisible"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ITaskResponse<TaskData>> ResolveRecaptchaV2Async(string siteKey, string url, bool invisible = false, CancellationToken cancellationToken = default)
        {
            var result = await Build()
                .WithUrlGet(new UrlBuilder(EndPoint, "recaptchav2")
                    .WithParam("apikey", ApiKey)
                    .WithParam("sitekey", siteKey)
                    .WithParam("siteurl", url)
                    .WithParam("invisible", invisible))
                .ExecuteAsync<TaskResponse<TaskData>>(cancellationToken);
            result.FirstCaptchaApi = this;
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ITaskResult<TData>> GetTaskResultAsync<TData>(long taskId, CancellationToken cancellationToken = default)
        {
            var result = await Build()
                .WithUrlGet(new UrlBuilder(EndPoint, "getResult")
                    .WithParam("apikey", ApiKey)
                    .WithParam("taskid", taskId))
                .ExecuteAsync<TaskResult<TData>>(cancellationToken);
            return result;
        }




        /// <summary>
        /// 
        /// </summary>
        public interface ITaskResponse<TData>
        {
            /// <summary>
            /// 
            /// </summary>
            int Code { get; }
            /// <summary>
            /// 
            /// </summary>
            long TaskId { get; }
            /// <summary>
            /// 
            /// </summary>
            string Message { get; }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            Task<ITaskResult<TData>> GetTaskResultAsync(CancellationToken cancellationToken = default);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="delay"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            Task<ITaskResult<TData>> GetTaskResultAsync(int delay, CancellationToken cancellationToken = default);
        }
        private class TaskResponse<TData> : ITaskResponse<TData>
        {
            [JsonIgnore]
            internal FirstCaptchaApi FirstCaptchaApi { get; set; }
            public int Code { get; set; }
            public long TaskId { get; set; }
            public string Message { get; set; }

            public Task<ITaskResult<TData>> GetTaskResultAsync(CancellationToken cancellationToken = default)
                => GetTaskResultAsync(2000, cancellationToken);
            public async Task<ITaskResult<TData>> GetTaskResultAsync(int delay, CancellationToken cancellationToken = default)
            {
                while (true)
                {
                    await Task.Delay(delay, cancellationToken);
                    ITaskResult<TData> taskResult = await FirstCaptchaApi.GetTaskResultAsync<TData>(this.TaskId, cancellationToken);
                    switch (taskResult.Status)
                    {
                        case TaskResultStatus.SUCCESS:
                        case TaskResultStatus.ERROR:
                            return taskResult;
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public interface ITaskResult<T>
        {
            /// <summary>
            /// 
            /// </summary>
            int Code { get; }
            /// <summary>
            /// 
            /// </summary>
            TaskResultStatus Status { get; }
            /// <summary>
            /// 
            /// </summary>
            string Message { get; }
            /// <summary>
            /// 
            /// </summary>
            T Data { get; }
        }
        private class TaskResult<TData> : ITaskResult<TData>
        {
            [JsonProperty("Code")]
            public int Code { get; set; }

            [JsonProperty("Status")]
            public TaskResultStatus Status { get; set; }

            [JsonProperty("Message")]
            public string Message { get; set; }

            [JsonProperty("Data")]
            public TData Data { get; set; }
        }
        /// <summary>
        /// 
        /// </summary>
        public class TaskData
        {
            /// <summary>
            /// 
            /// </summary>
            public string Token { get; set; }
        }
        /// <summary>
        /// 
        /// </summary>
        public enum TaskResultStatus
        {
            /// <summary>
            /// 
            /// </summary>
            PENDING,
            /// <summary>
            /// 
            /// </summary>
            PROCESSING,
            /// <summary>
            /// 
            /// </summary>
            SUCCESS,
            /// <summary>
            /// 
            /// </summary>
            ERROR
        }
    }
}
