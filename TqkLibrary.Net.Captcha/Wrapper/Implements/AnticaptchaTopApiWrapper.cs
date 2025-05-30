using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TqkLibrary.Net.Captcha.Services;

namespace TqkLibrary.Net.Captcha.Wrapper.Implements
{
    /// <summary>
    /// 
    /// </summary>
    public class AnticaptchaTopApiWrapper : ICaptchaWrapper
    {
        readonly AnticaptchaTopApi _anticaptchaTopApi;
        /// <summary>
        /// 
        /// </summary>
        public bool IsInvisible { get; set; } = false;
        /// <summary>
        /// 
        /// </summary>
        public AnticaptchaTopApi.ImageType ImageType { get; set; } = AnticaptchaTopApi.ImageType.Any;
        /// <summary>
        /// 
        /// </summary>
        public bool IsCalc { get; set; } = false;
        /// <summary>
        /// 
        /// </summary>
        public bool IsNumeric { get; set; } = false;
        /// <summary>
        /// 
        /// </summary>
        public bool IsCasesensitive { get; set; } = false;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="anticaptchaTopApi"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public AnticaptchaTopApiWrapper(AnticaptchaTopApi anticaptchaTopApi)
        {
            this._anticaptchaTopApi = anticaptchaTopApi ?? throw new ArgumentNullException(nameof(anticaptchaTopApi));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        public AnticaptchaTopApiWrapper(string apiKey)
        {
            this._anticaptchaTopApi = new AnticaptchaTopApi(apiKey);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="siteKey"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ICaptchaTask<BasicCaptchaTaskResult>> CreateRecaptchaV2TaskAsync(
            RecaptchaV2DataRequest recaptchaV2DataRequest,
            CancellationToken cancellationToken = default
            )
        {
            if (recaptchaV2DataRequest is null)
                throw new ArgumentNullException(nameof(recaptchaV2DataRequest));

            var task = await _anticaptchaTopApi.RecaptchaV2Async(
                recaptchaV2DataRequest.DataSiteKey,
                recaptchaV2DataRequest.PageUrl,
                recaptchaV2DataRequest.IsInvisible == true,
                cancellationToken
                );
            return new ReCaptchaTask(task, _anticaptchaTopApi);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bitmapBuffer"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ICaptchaTask<BasicCaptchaTaskResult>> CreateImageCaptchaTaskAsync(byte[] bitmapBuffer, CancellationToken cancellationToken = default)
        {
            AnticaptchaTopApi.ImageToTextOption imageToTextOption = bitmapBuffer;
            imageToTextOption.ImageType = ImageType;
            imageToTextOption.IsCalc = IsCalc;
            imageToTextOption.IsNumeric = IsNumeric;
            imageToTextOption.IsCasesensitive = IsCasesensitive;
            var result = await _anticaptchaTopApi.ImageToTextAsync(imageToTextOption, cancellationToken);
            return new ImageToTextCaptchaTask(result);
        }

        class ReCaptchaTask : ICaptchaTask<BasicCaptchaTaskResult>
        {
            readonly AnticaptchaTopApi.RecaptchaV2Response _response;
            readonly AnticaptchaTopApi _anticaptchaTopApi;
            public ReCaptchaTask(AnticaptchaTopApi.RecaptchaV2Response response, AnticaptchaTopApi anticaptchaTopApi)
            {
                _response = response ?? throw new ArgumentNullException(nameof(response));
                _anticaptchaTopApi = anticaptchaTopApi ?? throw new ArgumentNullException(nameof(anticaptchaTopApi));
            }
            public Task<BasicCaptchaTaskResult> GetTaskResultAsync(int delay = 2000, CancellationToken cancellationToken = default)
            {
                return Task.FromResult(new BasicCaptchaTaskResult()
                {
                    IsSuccess = _response.Success == true,
                    Value = _response.Captcha!,
                    ErrorMessage = _response.Message!,
                });
            }
        }

        class ImageToTextCaptchaTask : ICaptchaTask<BasicCaptchaTaskResult>
        {
            readonly AnticaptchaTopApi.ImageToTextResponse _imageToTextResponse;
            public ImageToTextCaptchaTask(AnticaptchaTopApi.ImageToTextResponse imageToTextResponse)
            {
                this._imageToTextResponse = imageToTextResponse ?? throw new ArgumentNullException(nameof(imageToTextResponse));
            }
            public Task<BasicCaptchaTaskResult> GetTaskResultAsync(int delay = 2000, CancellationToken cancellationToken = default)
            {
                return Task.FromResult(new BasicCaptchaTaskResult()
                {
                    IsSuccess = _imageToTextResponse.Success,
                    Value = _imageToTextResponse.Captcha!,
                    ErrorMessage = _imageToTextResponse.Message!,
                });
            }
        }
    }
}
