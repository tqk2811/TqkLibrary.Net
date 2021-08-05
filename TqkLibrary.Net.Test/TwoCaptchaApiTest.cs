using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using TqkLibrary.Net.Captcha;

namespace TqkLibrary.Net.Test
{
  [TestClass]
  public class TwoCaptchaApiTest
  {
    [TestMethod]
    public void TestNomal()
    {
      ApiKeys apiKeys = ApiKeys.Read();
      TwoCaptchaApi api = new TwoCaptchaApi(apiKeys.TwoCaptchaKey);
      //Bitmap bitmap = (Bitmap)
    }
  }
}
