using Newtonsoft.Json;

namespace TqkLibrary.Net.Captcha.AntiCaptchaCom
{
  public sealed class AntiCaptchaTaskResultResponse
  {
    [JsonProperty("errorId")]
    public int? ErrorId { get; set; }

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
}