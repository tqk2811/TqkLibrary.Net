using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.FptAi
{
  public class TextToSpeech : BaseApi
  {
    const string EndPoint = "https://api.fpt.ai/hmi/tts";
    readonly string version = "/v5";
    public TextToSpeech(string ApiKey) : base(ApiKey)
    {

    }

    public async Task<TTSResponse> TTS(string text, Voice voice = Voice.BanMai, Speed speed = Speed.Normal, Format format = Format.mp3, CancellationToken cancellationToken = default)
    {
      if (string.IsNullOrWhiteSpace(text)) throw new ArgumentNullException(nameof(text));

      using HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, EndPoint + version);
      httpRequestMessage.Headers.Add("api-key", ApiKey);
      httpRequestMessage.Headers.Add("speed", ((int)speed).ToString());
      httpRequestMessage.Headers.Add("voice", voice.ToString().ToLower());
      httpRequestMessage.Headers.Add("format", format.ToString().ToLower());
      using StringContent stringContent = new StringContent(text, Encoding.UTF8, "text/html");
      httpRequestMessage.Content = stringContent;
      return await RequestPost<TTSResponse>(httpRequestMessage, cancellationToken).ConfigureAwait(false);
    }
  }
}
