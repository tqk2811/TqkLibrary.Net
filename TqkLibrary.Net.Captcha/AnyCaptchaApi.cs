using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Captcha
{
    /// <summary>
    /// 
    /// </summary>
    public class AnyCaptchaApi : BaseApi
    {
        const string EndPoint = "https://api.anycaptcha.com";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        public AnyCaptchaApi(string apiKey) : base(apiKey) { }

        async Task<IAnyCaptchaTaskResponse> RequestTask(CreateTaskRequest createTaskRequest, CancellationToken cancellationToken = default)
        {
            AnyCaptchaTaskResponse responseTask = await Build()
                .WithUrlPostJson(new UriBuilder(EndPoint, "createTask"), createTaskRequest)
                .ExecuteAsync<AnyCaptchaTaskResponse>(cancellationToken);
            responseTask.anyCaptchaApi = this;
            return responseTask;
        }


        public Task<IAnyCaptchaTaskResponse> ImageToTextAsync(byte[] imageBuffer, CancellationToken cancellationToken = default)
        {
            if (imageBuffer is null || imageBuffer.Length == 0) throw new ArgumentNullException(nameof(imageBuffer));
            CreateTaskRequest createTaskData = new CreateTaskRequest()
            {
                ClientKey = this.ApiKey,
                Task = new TaskData()
                {
                    Type = "ImageToTextTask",
                    Body = Convert.ToBase64String(imageBuffer),
                }
            };

            return RequestTask(createTaskData, cancellationToken);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<IAnyCaptchaTaskResponse> RecaptchaV2TaskProxylessAsync(
            string websiteURL,
            string websiteKey,
            bool isInvisible = false,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(websiteKey)) throw new ArgumentNullException(nameof(websiteKey));
            Uri uri = new Uri(websiteURL);//check format

            CreateTaskRequest createTaskData = new CreateTaskRequest()
            {
                ClientKey = this.ApiKey,
                Task = new TaskData()
                {
                    Type = "RecaptchaV2TaskProxyless",
                    WebsiteURL = uri.ToString(),
                    WebsiteKey = websiteKey,
                    IsInvisible = isInvisible
                }
            };

            return RequestTask(createTaskData, cancellationToken);
        }



        internal Task<GetTaskResultResponse> GetTaskResult(IAnyCaptchaTaskResponse responseTask, CancellationToken cancellationToken = default)
        {
            if (responseTask.ErrorId != 0) throw new InvalidOperationException("Task was create failed");

            GetTaskResultRequest getTaskResultData = new GetTaskResultRequest()
            {
                ClientKey = ApiKey,
                TaskId = responseTask.TaskId,
            };

            return Build()
                .WithUrlPostJson(new UriBuilder(EndPoint, "getTaskResult"), getTaskResultData)
                .ExecuteAsync<GetTaskResultResponse>(cancellationToken);
        }








        class GetTaskResultRequest
        {
            [JsonProperty("clientKey")]
            public string ClientKey { get; set; }

            [JsonProperty("taskId")]
            public long TaskId { get; set; }
        }

        class CreateTaskRequest
        {
            [JsonProperty("clientKey")]
            public string ClientKey { get; set; }

            [JsonProperty("task")]
            public TaskData Task { get; set; }

        }
        class TaskData
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("websiteURL")]
            public string WebsiteURL { get; set; }

            [JsonProperty("websiteKey")]
            public string WebsiteKey { get; set; }

            [JsonProperty("isInvisible")]
            public bool IsInvisible { get; set; }

            [JsonProperty("body")]
            public string Body { get; set; }
        }
    }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface IAnyCaptchaTaskResponse
    {
        string ErrorCode { get; }
        string ErrorDescription { get; }
        int ErrorId { get; }
        long TaskId { get; }
        Task<GetTaskResultResponse> WaitForResultAsync(int delay = 5000, CancellationToken cancellationToken = default);
    }
    public class GetTaskResultResponse
    {
        [JsonProperty("errorId")]
        public int ErrorId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("errorCode")]
        public string ErrorCode { get; set; }

        [JsonProperty("errorDescription")]
        public string ErrorDescription { get; set; }

        [JsonProperty("solution")]
        public GetTaskResultResponseSolution Solution { get; set; }
    }

    public class GetTaskResultResponseSolution
    {
        [JsonProperty("gRecaptchaResponse")]
        public string GRecaptchaResponse { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    internal class AnyCaptchaTaskResponse : IAnyCaptchaTaskResponse
    {
        [JsonProperty("errorId")]
        public int ErrorId { get; set; }

        [JsonProperty("errorCode")]
        public string ErrorCode { get; set; }

        [JsonProperty("errorDescription")]
        public string ErrorDescription { get; set; }

        [JsonProperty("taskId")]
        public long TaskId { get; set; }

        internal AnyCaptchaApi anyCaptchaApi;

        public async Task<GetTaskResultResponse> WaitForResultAsync(int delay = 5000, CancellationToken cancellationToken = default)
        {
            while (true)
            {
                GetTaskResultResponse getTaskResultResponse = await anyCaptchaApi.GetTaskResult(this, cancellationToken).ConfigureAwait(false);
                if (getTaskResultResponse.ErrorId != 0 || "ready".Equals(getTaskResultResponse.Status)) return getTaskResultResponse;
                await Task.Delay(delay, cancellationToken);
            }
        }
    }

}
