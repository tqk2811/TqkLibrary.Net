using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TqkLibrary.Net.Others.FptAi;

namespace TqkLibrary.Net.Test
{
  [TestClass]
  public class FptAiTest
  {
    [TestMethod]
    public void TextToSpeechTest()
    {
      string text = @"Áo sơ mi tay dài nữ - Lụa basic - Bạc grey
- Chất liệu : Lụa cao cấp
- Màu sắc : Trắng - Hồng - Bạc ( Silver Grey - Silver Gold)
- Kiểu dáng : Áo somi tay dài basic
- Loại hàng : Áo somi tay dài basic đi làm, đi chơi
- Kích thước : Size S/M
Tay : 55
Vai : 42
Ngực : 86
Dài : 45
Eo : 84
-----------------
Mọi người có thể FOLLOW mình tại đây nhé!!!";
      TextToSpeech textToSpeech = new TextToSpeech("IYvSNfjOQPSVKg7KZsBb2oFXTI0yDiD8");
      TTSResponse response = textToSpeech.TTS(text).Result;
    }
  }
}
