using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TqkLibrary.Net.PhoneNumberApi.OtpSimCom
{
  public class PhoneData
  {
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("phone_number")]
    public string PhoneNumber { get; set; }

    [JsonProperty("service_id")]
    public int ServiceId { get; set; }

    [JsonProperty("service_name")]
    public string ServiceName { get; set; }

    [JsonProperty("status")]
    public PhoneDataStatus Status { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty("done_at")]
    public DateTime DoneAt { get; set; }

    [JsonProperty("messages")]
    public List<PhoneDataMessage> Messages { get; set; }
  }

  public enum PhoneDataStatus
  {
    Waiting = 1,
    Completed = 0,
    Expired = 2
  }

  public class PhoneDataMessage
  {
    [JsonProperty("sms_from")]
    public string SmsFrom { get; set; }

    [JsonProperty("sms_content")]
    public string SmsContent { get; set; }

    [JsonProperty("is_audio")]
    public bool IsAudio { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty("otp")]
    public string Otp { get; set; }

    [JsonProperty("audio_file")]
    public string AudioFile { get; set; }

    [JsonProperty("audio_content")]
    public string AudioContent { get; set; }
  }
}