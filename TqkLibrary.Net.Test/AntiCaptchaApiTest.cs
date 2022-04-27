using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using System.Threading.Tasks;
using TqkLibrary.Net.Captcha;

namespace TqkLibrary.Net.Test
{
  [TestClass]
  public class AntiCaptchaApiTest
  {
    [TestMethod]
    public void TestNomal()
    {
      ApiKeys apiKeys = ApiKeys.Read();
      System.Drawing.Bitmap bitmap = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile("D:\\test.jpeg");
      AntiCaptchaApi antiCaptchaApi = new AntiCaptchaApi(apiKeys.AntiCaptchaKey);
      var task_res = antiCaptchaApi.CreateTask(bitmap.AntiCaptchaImageToTextTask()).Result;
      if (task_res.ErrorId != 0) 
        throw new Exception($"AntiCaptcha.CreateTask: {task_res.ErrorCode}, {task_res.ErrorDescription}");

      while (true)
      {
        var task_resolve = antiCaptchaApi.GetTaskResult(task_res).Result;
        if (task_resolve.ErrorId != 0) 
          throw new Exception($"AntiCaptcha.GetTaskResult: {task_resolve.ErrorCode}, {task_resolve.ErrorDescription}");

        if (!string.IsNullOrEmpty(task_resolve.Solution?.Text))
        {

        }
        else Task.Delay(2000).Wait();
      }
    }
  }
}
