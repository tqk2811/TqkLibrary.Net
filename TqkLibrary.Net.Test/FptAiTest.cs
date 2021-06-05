using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TqkLibrary.Net.FptAi;

namespace TqkLibrary.Net.Test
{
  [TestClass]
  public class FptAiTest
  {
    [TestMethod]
    public void TextToSpeechTest()
    {
      TextToSpeech textToSpeech = new TextToSpeech("IYvSNfjOQPSVKg7KZsBb2oFXTI0yDiD8");
      TTSResponse response = textToSpeech.TTS("Nguyễn Hải Long").Result;
    }
  }
}
