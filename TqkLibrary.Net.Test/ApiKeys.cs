using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Test
{
  class ApiKeys
  {
    public string AntiCaptchaKey { get; set; }
    public string TwoCaptchaKey { get; set; }
    public string OtpSimKey { get; set; }
    public string ChoThueSimCodeKey { get; set; }


    public static ApiKeys Read()
    {
      return JsonConvert.DeserializeObject<ApiKeys>(File.ReadAllText("Key.json"));
    }
  }
}
