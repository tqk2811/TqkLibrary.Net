﻿using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Captcha
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class TwoCaptchaApi : BaseApi
    {
        private const string EndPoint = "https://2captcha.com";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ApiKey"></param>
        public TwoCaptchaApi(string ApiKey) : base(ApiKey)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public Task<TwoCaptchaResponse> GetResponseJson(string id, CancellationToken cancellationToken = default)
            => Build()
            .WithUrlGet(new UriBuilder(EndPoint, "res.php")
                .WithParam("key", ApiKey)
                .WithParam("id", id)
                .WithParam("action", "get")
                .WithParam("json", 1))
            .ExecuteAsync<TwoCaptchaResponse>(cancellationToken);

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <param name="delay"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="HttpRequestException"></exception>
        /// <exception cref="JsonException"></exception>
        /// <exception cref="OperationCanceledException"></exception>
        public async Task<TwoCaptchaResponse> WaitResponseJsonCompleted(string id, int delay = 5000, CancellationToken cancellationToken = default)
        {
            while (true)
            {
                await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
                TwoCaptchaResponse twoCaptchaResponse = await GetResponseJson(id).ConfigureAwait(false);
                switch (twoCaptchaResponse.CheckState())
                {
                    case TwoCaptchaState.NotReady:
                        continue;
                    case TwoCaptchaState.Error:
                    case TwoCaptchaState.Success:
                        return twoCaptchaResponse;
                }
            }
        }

        //https://2captcha.com/2captcha-api#solving_recaptchav2_old
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<TwoCaptchaResponse> ReCaptchaV2_old(
            byte[] bitmapBuffer, byte[] imginstructionsBuffer,
            int? recaptcharows = null, int? recaptchacols = null,
            CancellationToken cancellationToken = default)
        {
            if (bitmapBuffer is null || bitmapBuffer.Length == 0) throw new ArgumentNullException(nameof(bitmapBuffer));
            if (imginstructionsBuffer is null || imginstructionsBuffer.Length == 0) throw new ArgumentNullException(nameof(imginstructionsBuffer));

            MultipartFormDataContent requestContent = new MultipartFormDataContent();
            ByteArrayContent imageContent_bitmap = new ByteArrayContent(bitmapBuffer);
            imageContent_bitmap.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
            requestContent.Add(imageContent_bitmap, "file", "file.jpg");
            ByteArrayContent imageContent_instructions = new ByteArrayContent(imginstructionsBuffer);
            imageContent_instructions.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
            requestContent.Add(imageContent_instructions, "imginstructions", "imginstructions.jpg");


            return Build()
                .WithUrlPost(
                    new UriBuilder(EndPoint, "in.php")
                        .WithParam("key", ApiKey)
                        .WithParam("method", "post")
                        .WithParam("json", 1)
                        .WithParam("recaptcha", 1)
                        .WithParamIfNotNull("recaptcharows", recaptcharows)
                        .WithParamIfNotNull("recaptchacols", recaptchacols),
                    requestContent)
                .ExecuteAsync<TwoCaptchaResponse>(cancellationToken);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<TwoCaptchaResponse> Nomal(byte[] bitmap, CancellationToken cancellationToken = default)
        {
            MultipartFormDataContent requestContent = new MultipartFormDataContent();
            ByteArrayContent imageContent_bitmap = new ByteArrayContent(bitmap);
            imageContent_bitmap.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
            requestContent.Add(imageContent_bitmap, "file", "file.jpg");

            return Build()
                .WithUrlPost(new UriBuilder(EndPoint, "in.php")
                    .WithParam("key", ApiKey)
                    .WithParam("method", "post")
                    .WithParam("json", 1),
                    requestContent)
                .ExecuteAsync<TwoCaptchaResponse>(cancellationToken);
        }

        //https://2captcha.com/2captcha-api#recaptchav2new_proxy
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<TwoCaptchaResponse> RecaptchaV2(
            string googleKey, string pageUrl, string cookies = null, string proxy = null, string proxytype = null,
            CancellationToken cancellationToken = default)
            => Build()
                .WithUrlGet(new UriBuilder(EndPoint, "in.php")
                    .WithParam("key", ApiKey)
                    .WithParam("method", "userrecaptcha")
                    .WithParam("json", 1)
                    .WithParam("googlekey", googleKey)
                    .WithParam("pageurl", pageUrl)
                    .WithParamIfNotNull("cookies", cookies)
                    .WithParamIfNotNull("proxy", proxy)
                    .WithParamIfNotNull("proxytype", proxytype))
                .ExecuteAsync<TwoCaptchaResponse>(cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<TwoCaptchaResponse> RecaptchaV2Invisible(
            string googleKey, string pageUrl, string cookies = null, string proxy = null, string proxytype = null,
            CancellationToken cancellationToken = default)
            => Build()
                .WithUrlGet(new UriBuilder(EndPoint, "in.php")
                    .WithParam("key", ApiKey)
                    .WithParam("method", "userrecaptcha")
                    .WithParam("json", 1)
                    .WithParam("invisible",1)
                    .WithParam("googlekey", googleKey)
                    .WithParam("pageurl", pageUrl)
                    .WithParamIfNotNull("cookies", cookies)
                    .WithParamIfNotNull("proxy", proxy)
                    .WithParamIfNotNull("proxytype", proxytype))
                .ExecuteAsync<TwoCaptchaResponse>(cancellationToken);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<TwoCaptchaResponse> RecaptchaV3(
            string googleKey, string pageUrl, float minScore = 0.3f,
            CancellationToken cancellationToken = default)
            => Build()
                .WithUrlGet(new UriBuilder(EndPoint, "in.php")
                    .WithParam("key", ApiKey)
                    .WithParam("method", "userrecaptcha")
                    .WithParam("json", 1)
                    .WithParam("version", "v3")
                    .WithParam("action", "verify")
                    .WithParam("min_score", minScore)
                    .WithParam("googlekey", googleKey)
                    .WithParam("pageurl", pageUrl))
                .ExecuteAsync<TwoCaptchaResponse>(cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<TwoCaptchaResponse> Coordinates(byte[] bitmap, CancellationToken cancellationToken = default)
        {
            MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();
            ByteArrayContent byteArrayContent = new ByteArrayContent(bitmap);
            multipartFormDataContent.Add(byteArrayContent, "file");
            return Build()
                .WithUrlPost(new UriBuilder(EndPoint, "in.php")
                    .WithParam("key", ApiKey)
                    .WithParam("method", "post")
                    .WithParam("json", 1)
                    .WithParam("coordinatescaptcha", "1"),
                    multipartFormDataContent)
                .ExecuteAsync<TwoCaptchaResponse>(cancellationToken);
        }







#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public enum TwoCaptchaState
        {
            NotReady,
            Error,
            Success
        }


        public class TwoCaptchaResponse
        {
            [JsonProperty("status")]
            public int Status { get; set; }

            [JsonProperty("request")]
            public string Request { get; set; }

            public override string ToString()
            {
                return $"request: {Status}, request: {Request}";
            }

            public TwoCaptchaState CheckState()
            {
                if (Status == 1) return TwoCaptchaState.Success;
                if (Request.Contains("CAPCHA_NOT_READY")) return TwoCaptchaState.NotReady;
                else return TwoCaptchaState.Error;
            }
        }

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}