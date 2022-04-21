using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Others.FptAi
{
    public class TextToSpeech : BaseApi
    {
        const string EndPoint = "https://api.fpt.ai/hmi/tts";
        readonly string version = "/v5";
        public TextToSpeech(string ApiKey) : base(ApiKey)
        {

        }

        public async Task<TTSResponse> TTS(string text, Voice voice = Voice.BanMai, Speed speed = Speed.Normal, Format format = Format.mp3)
        {
            if (string.IsNullOrWhiteSpace(text)) throw new ArgumentNullException(nameof(text));

            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("api-key", ApiKey);
            dict.Add("speed", ((int)speed).ToString());
            dict.Add("voice", voice.ToString().ToLower());
            dict.Add("format", format.ToString().ToLower());

            return await RequestPostAsync<TTSResponse>(
              EndPoint + version,
              dict,
              new StringContent(text, Encoding.UTF8, "text/html")).ConfigureAwait(false);
        }
    }
}
