using Newtonsoft.Json;
using System.Collections.Generic;

namespace TqkLibrary.Net.PhoneNumberApi.SimThueCom
{
  public class ServicesResult : BaseResult
  {
    [JsonProperty("services")]
    public List<ServiceResult> Services { get; set; }
  }

  public class ServiceResult
  {
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("price")]
    public double Price { get; set; }
  }
}