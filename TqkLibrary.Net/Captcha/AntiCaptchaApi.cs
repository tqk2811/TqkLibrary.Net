using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Captcha
{
  /// <summary>
  /// https://anti-captcha.com/apidoc/image
  /// </summary>
  public class AntiCaptchaTask
  {
    [JsonConverter(typeof(StringEnumConverter))]
    [JsonProperty("type")]
    public AntiCaptchaType Type { get; set; }

    [JsonProperty("websiteURL")]
    public string WebsiteUrl { get; set; }

    [JsonProperty("websiteKey")]
    public string WebsiteKey { get; set; }

    [JsonProperty("proxyType")]
    public string ProxyType { get; set; }

    [JsonProperty("proxyAddress")]
    public string ProxyAddress { get; set; }

    [JsonProperty("proxyPort")]
    public int? ProxyPort { get; set; }

    [JsonProperty("proxyLogin")]
    public string ProxyLogin { get; set; }

    [JsonProperty("proxyPassword")]
    public string ProxyPassword { get; set; }

    [JsonProperty("userAgent")]
    public string UserAgent { get; set; }

    [JsonProperty("cookies")]
    public string Cookies { get; set; }

    [JsonProperty("body")]
    public string Body { get; set; }

    [JsonProperty("funcaptchaApiJSSubdomain")]
    public string FunCaptchaApiJSSubDomain { get; set; }

    [JsonProperty("data")]
    public string Data { get; set; }

    [JsonProperty("websitePublicKey")]
    public string WebsitePublicKey { get; set; }

    [JsonProperty("gt")]
    public string GT { get; set; }

    [JsonProperty("challenge")]
    public string Challenge { get; set; }

    [JsonProperty("geetestApiServerSubdomain")]
    public string GeeTestApiServerSubdomain { get; set; }
  }
  public class AntiCaptchaTaskResponse
  {
    [JsonProperty("errorId")]
    public int ErrorId { get; set; }

    [JsonProperty("errorCode")]
    public string ErrorCode { get; set; }

    [JsonProperty("errorDescription")]
    public string ErrorDescription { get; set; }

    [JsonProperty("taskId")]
    public int? TaskId { get; set; }
  }
  public class AntiCaptchaTaskResultResponse
  {
    [JsonProperty("errorId")]
    public int ErrorId { get; set; }

    [JsonProperty("errorCode")]
    public string ErrorCode { get; set; }

    [JsonProperty("errorDescription")]
    public string ErrorDescription { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }

    [JsonProperty("solution")]
    public AntiCaptchaTaskSolutionResultResponse Solution { get; set; }

    [JsonProperty("cost")]
    public double? cost { get; set; }

    [JsonProperty("ip")]
    public string Ip { get; set; }

    [JsonProperty("createTime")]
    public long? CreateTime { get; set; }

    [JsonProperty("endTime")]
    public long? EndTime { get; set; }

    [JsonProperty("solveCount")]
    public int? SolveCount { get; set; }

    public bool IsComplete()
    {
      return Status == null || Status.Equals("ready");
    }
  }
  public class AntiCaptchaTaskSolutionResultResponse
  {
    [JsonProperty("text")]
    public string Text { get; set; }

    [JsonProperty("url")]
    public string Url { get; set; }

    public string gRecaptchaResponse { get; set; }
  }

  /// <summary>
  /// https://anti-captcha.com/apidoc/image
  /// </summary>
  public enum AntiCaptchaType
  {
    FunCaptchaTask,
    FunCaptchaTaskProxyless,
    ImageToTextTask,

    /// <summary>
    /// Recaptcha no proxy
    /// </summary>
    NoCaptchaTaskProxyless,

    /// <summary>
    /// Recaptcha with proxy
    /// </summary>
    NoCaptchaTask,

    /// <summary>
    /// recaptcha V3 No proxy
    /// </summary>
    RecaptchaV3TaskProxyless,

    GeeTestTaskProxyless,
    GeeTestTask,
    HCaptchaTask,
    HCaptchaTaskProxyless
  }


  public class AntiCaptchaApi : BaseApi
  {
    private class CreateTaskJson
    {
      [JsonProperty("clientKey")]
      public string ClientKey { get; set; }

      [JsonProperty("task")]
      public AntiCaptchaTask Task { get; set; }

      [JsonProperty("softId")]
      public int? SoftId { get; set; }

      [JsonProperty("languagePool")]
      public string LanguagePool { get; set; } = "en";

      [JsonProperty("callbackUrl")]
      public string CallbackUrl { get; set; }
    }
    private class TaskResultJson
    {
      [JsonProperty("clientKey")]
      public string ClientKey { get; set; }

      [JsonProperty("taskId")]
      public int TaskId { get; set; }
    }


    private const string EndPoint = "https://api.anti-captcha.com";

    /// <summary>
    ///
    /// </summary>
    /// <exception cref="System.ArgumentNullException"></exception>
    /// <param name="ApiKey">ApiKey</param>
    public AntiCaptchaApi(string ApiKey, CancellationToken cancellationToken = default) : base(ApiKey, cancellationToken)
    {
    }

    public Task<AntiCaptchaTaskResponse> CreateTask(AntiCaptchaTask antiCaptchaTask, string languagePool = "en")
    {
      CreateTaskJson createTaskJson = new CreateTaskJson();
      createTaskJson.ClientKey = ApiKey;
      createTaskJson.Task = antiCaptchaTask;
      createTaskJson.LanguagePool = languagePool;

      string json = JsonConvert.SerializeObject(createTaskJson, NetExtensions.JsonSerializerSettings);
      return RequestPost<AntiCaptchaTaskResponse>(
        EndPoint + "/createTask",
        null,
        new StringContent(json, Encoding.UTF8, "application/json"));
    }

    public Task<AntiCaptchaTaskResultResponse> GetTaskResult(AntiCaptchaTaskResponse task)
    {
      TaskResultJson taskResultJson = new TaskResultJson();
      taskResultJson.ClientKey = ApiKey;
      taskResultJson.TaskId = task.TaskId.Value;

      string json = JsonConvert.SerializeObject(taskResultJson, NetExtensions.JsonSerializerSettings);
      return RequestPost<AntiCaptchaTaskResultResponse>(
        EndPoint + "/getTaskResult",
        null,
        new StringContent(json, Encoding.UTF8, "application/json"));
    }
  }

  public static class AntiCaptchaHelper
  {
    public static AntiCaptchaTask AntiCaptchaImageToTextTask(this Bitmap bitmap)
    {
      AntiCaptchaTask task = new AntiCaptchaTask();
      task.Type = AntiCaptchaType.ImageToTextTask;
      task.Body = Convert.ToBase64String(bitmap.BitmapToBuffer());
      return task;
    }
  }
}