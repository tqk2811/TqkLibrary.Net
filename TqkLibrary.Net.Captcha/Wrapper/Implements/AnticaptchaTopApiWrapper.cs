﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Captcha.Wrapper.Implements
{
    internal class AnticaptchaTopApiWrapper : ICaptchaWrapper
    {
        readonly AnticaptchaTopApi _anticaptchaTopApi;
        public bool IsInvisible { get; set; } = false;
        public AnticaptchaTopApi.ImageType ImageType { get; set; } = AnticaptchaTopApi.ImageType.Any;
        public bool IsCalc { get; set; } = false;
        public bool IsNumeric { get; set; } = false;
        public bool IsCasesensitive { get; set; } = false;
        public AnticaptchaTopApiWrapper(AnticaptchaTopApi anticaptchaTopApi)
        {
            this._anticaptchaTopApi = anticaptchaTopApi ?? throw new ArgumentNullException(nameof(anticaptchaTopApi));
        }
        public AnticaptchaTopApiWrapper(string apiKey)
        {
            this._anticaptchaTopApi = new AnticaptchaTopApi(apiKey);
        }
        public async Task<ICaptchaTask<BasicCaptchaTaskResult>> CreateGoogleRecaptchaV2TaskAsync(string url, string siteKey, CancellationToken cancellationToken = default)
        {
            var task = await _anticaptchaTopApi.RecaptchaV2Async(siteKey, url, IsInvisible, cancellationToken);
            return new ReCaptchaTask(task, _anticaptchaTopApi);
        }

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
            readonly AnticaptchaTopApi.TaskResponse _taskResponse;
            readonly AnticaptchaTopApi _anticaptchaTopApi;
            public ReCaptchaTask(AnticaptchaTopApi.TaskResponse taskResponse, AnticaptchaTopApi anticaptchaTopApi)
            {
                _taskResponse = taskResponse ?? throw new ArgumentNullException(nameof(taskResponse));
                _anticaptchaTopApi = anticaptchaTopApi ?? throw new ArgumentNullException(nameof(anticaptchaTopApi));
            }
            public async Task<BasicCaptchaTaskResult> GetTaskResultAsync(int delay = 2000, CancellationToken cancellationToken = default)
            {
                AnticaptchaTopApi.TaskResponse taskResponse = _taskResponse;
                if (!string.IsNullOrWhiteSpace(_taskResponse.Request))
                {
                    taskResponse = await _anticaptchaTopApi.WaitUntilTaskResultAsync(_taskResponse.Request!, delay, cancellationToken);
                }
                return new BasicCaptchaTaskResult()
                {
                    IsSuccess = taskResponse.Status == 1,
                    Value = taskResponse.Status == 1 ? taskResponse.Request! : string.Empty,
                    ErrorMessage = taskResponse.Status == 1 ? string.Empty : taskResponse.Request!,
                };
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