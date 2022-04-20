using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nito.AsyncEx;
using TqkLibrary.Utils;
using System.Text.RegularExpressions;

namespace TqkLibrary.Net.Mail.TempMail
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEmailTm
    {
        /// <summary>
        /// 
        /// </summary>
        public string FromAddress { get; }
        /// <summary>
        /// 
        /// </summary>
        public string Title { get; }
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<string> Files { get; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class MailTm : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public static int EmailMinLength { get; set; } = 5;
        /// <summary>
        /// 
        /// </summary>
        public static int EmailMaxLength { get; set; } = 10;
        /// <summary>
        /// 
        /// </summary>
        public static int PasswordMinLength { get; set; } = 8;
        /// <summary>
        /// 
        /// </summary>
        public static int PasswordMaxLength { get; set; } = 10;

        /// <summary>
        /// Create new instance of MailTm
        /// </summary>
        /// <returns></returns>
        public static Task<MailTm> NewInstanceAsync(CancellationToken cancellationToken = default)
        {
            return new MailTm().InitAsync(cancellationToken);
        }



        readonly HttpClientHandler httpClientHandler;
        readonly HttpClient httpClient;
        internal MailTm()
        {
            httpClientHandler = new HttpClientHandler();
            httpClientHandler.UseCookies = true;
            httpClientHandler.CookieContainer = new CookieContainer();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
            httpClientHandler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            httpClient = new HttpClient(httpClientHandler, false);
            httpClient.DefaultRequestHeaders.Referrer = new Uri("https://mail.tm/");
        }
        /// <summary>
        /// 
        /// </summary>
        ~MailTm()
        {
            Dispose(false);
        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool disposing)
        {
            httpClient.Dispose();
            httpClientHandler.Dispose();
            cancellationTokenSource.Dispose();
        }

        MailTmAccount mailTmAccount;
        MailTmToken token;

        /// <summary>
        /// 
        /// </summary>
        public string Email { get { return mailTmAccount?.address; } }

        readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        private async Task<MailTm> InitAsync(CancellationToken cancellationToken = default)
        {
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                //get mail domain
                HydraMember hydraMember = null;
                {
                    using HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, "https://api.mail.tm/domains");
                    using HttpResponseMessage rep = await httpClient.SendAsync(req, HttpCompletionOption.ResponseContentRead, cancellationToken).ConfigureAwait(false);
                    string body = await rep.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
                    MailTmDomain mailTmDomain = JsonConvert.DeserializeObject<MailTmDomain>(body);
                    hydraMember = mailTmDomain.HydraMembers.FirstOrDefault();
                    if (hydraMember == null)
                        continue;
                }

                mailTmAccount = new MailTmAccount()
                {
                    address = $"{RandomStrings.RandomStringLower(EmailMinLength, EmailMaxLength)}@{hydraMember.Domain}",
                    password = RandomStrings.RandomStringAndNum(PasswordMinLength, PasswordMaxLength),
                };
                //create account
                {
                    using HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, "https://api.mail.tm/accounts");
                    using StringContent stringContent = new StringContent(JsonConvert.SerializeObject(mailTmAccount), Encoding.UTF8, "application/json");
                    req.Content = stringContent;
                    using HttpResponseMessage rep = await httpClient.SendAsync(req, HttpCompletionOption.ResponseContentRead, cancellationToken).ConfigureAwait(false);
                    string body = await rep.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
                    MailTmAccount mailTmAccount2 = JsonConvert.DeserializeObject<MailTmAccount>(body);
                    if (!mailTmAccount.address.Equals(mailTmAccount2.address))
                        continue;
                }

                //login account
                {
                    using HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, "https://api.mail.tm/token");
                    using StringContent stringContent = new StringContent(JsonConvert.SerializeObject(mailTmAccount), Encoding.UTF8, "application/json");
                    req.Content = stringContent;
                    using HttpResponseMessage rep = await httpClient.SendAsync(req, HttpCompletionOption.ResponseContentRead, cancellationToken).ConfigureAwait(false);
                    string body = await rep.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
                    token = JsonConvert.DeserializeObject<MailTmToken>(body);
                    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.Token}");
                }
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/event-stream"));
                return this;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event Action<IEmailTm> OnMailReceived;

        /// <summary>
        /// 
        /// </summary>
        public Task StartListen()
        {
            return Task.Factory.StartNew(() =>
            {
                AsyncContext.Run(async () => await GetMailAsync());
            }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }
        /// <summary>
        /// 
        /// </summary>
        public void StopListen()
        {
            cancellationTokenSource.Cancel();
        }

        async Task GetMailAsync()
        {
            try
            {
                while (true)
                {
                    using HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, $"https://mercure.mail.tm/.well-known/mercure?topic=/accounts/{token.Id}");
                    using HttpResponseMessage rep = await httpClient.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, cancellationTokenSource.Token).ConfigureAwait(false);
                    using Stream stream = await rep.EnsureSuccessStatusCode().Content.ReadAsStreamAsync().ConfigureAwait(false);
                    using StreamReader streamReader = new StreamReader(stream);
                    while (!streamReader.EndOfStream)
                    {
                        string line = await streamReader.ReadLineAsync().ConfigureAwait(false);
                        if (line.StartsWith("data:"))
                        {
                            string json = line.Substring(line.IndexOf("{"));
                            Rootobject mailTmMail = JsonConvert.DeserializeObject<Rootobject>(json);
                            if (mailTmMail.from != null)
                            {
                                var files = await DownloadMail(mailTmMail.downloadUrl).ConfigureAwait(false);
                                EmailTm_ emailTm = new EmailTm_()
                                {
                                    Files_ = files,
                                    FromAddress = mailTmMail.from.address,
                                    //Title = mailTmMail.n
                                };
                                ThreadPool.QueueUserWorkItem((x) => OnMailReceived?.Invoke(emailTm));
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }


        async Task<List<string>> DownloadMail(string downloadUrl)
        {
            using HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, $"https://api.mail.tm{downloadUrl}");
            using HttpResponseMessage rep = await httpClient.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, cancellationTokenSource.Token).ConfigureAwait(false);

            string content = await rep.EnsureSuccessStatusCode().Content.ReadAsStringAsync();

            using MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(content));
            ms.Position = 0;
            using StreamReader streamReader = new StreamReader(ms);

            bool isLastEmpty = false;
            bool found = false;
            string boundary = string.Empty;
            while (!streamReader.EndOfStream && !found)
            {
                string line = streamReader.ReadLine();
                bool isCurrentEmpty = string.IsNullOrEmpty(line);
                if(isCurrentEmpty)
                {
                    if (isLastEmpty)
                    {
                        if (isCurrentEmpty) found = true;
                    }
                    else isLastEmpty = isCurrentEmpty;
                }
                else
                {
                    if(line.StartsWith("Content-Type"))
                    {
                        Match match = Regex.Match(line, "boundary=\\\"(.*?)\\\"");
                        if (match.Success) boundary = match.Groups[1].Value.TrimEnd('\\');
                    }
                }
            }
            List<string> results = new List<string>();
            if (found)
            {
                string body = streamReader.ReadToEnd();
                if (!string.IsNullOrEmpty(boundary))
                {
                    //fix boundary
                    body = Regex.Replace(body, $"{boundary}[^\\r-]", $"{boundary}C\r\n");
                }
                using MemoryStream ms2 = new MemoryStream(Encoding.UTF8.GetBytes(body));
                ms2.Position = 0;
                var p = HttpMultipartParser.MultipartFormDataParser.Parse(ms2);
                foreach (var file in p.Files)
                {
                    if (file.ContentType.Contains("text"))
                    {
                        using MemoryStream msFile = new MemoryStream();
                        file.Data.CopyTo(msFile);
                        results.Add(Encoding.UTF8.GetString(msFile.ToArray()));
                    }
                }
            }
            return results;
        }





#if DEBUG

        const string a = @"Delivered-To: qscski@candassociates.com
Return-Path: <tqk2811@gmail.com>
Received: from mail-qv1-xf29.google.com (mail-qv1-xf29.google.com [2607:f8b0:4864:20::f29])
	by in.mail.tm (Haraka/2.8.28) with ESMTPS id 1F374EDF-2DBA-495C-90D4-E4234744C82F.1
	envelope-from <tqk2811@gmail.com>
	tls TLS_ECDHE_RSA_WITH_AES_256_GCM_SHA384;
	Tue, 19 Apr 2022 11:26:30 +0000
Received: by mail-qv1-xf29.google.com with SMTP id b17so12813003qvf.12
        for <qscski@candassociates.com>; Tue, 19 Apr 2022 04:26:30 -0700 (PDT)
DKIM-Signature: v=1; a=rsa-sha256; c=relaxed/relaxed;
        d=gmail.com; s=20210112;
        h=mime-version:from:date:message-id:subject:to;
        bh=ag9uumXWLjZpIPOuU+KsLUBIZ85kfwVB37B15cJYa+E=;
        b=hR1J5LvbxxHa4Kt0lDz07bJ6RrUYSplgqpYG26AnSxLTU3zNnksptX4QYIc5CmApDE
         Ap5wkVSXUTESTiMJ4lrYcVHi4PdU6a5bDJmDrsKxZn/XRifgOkfm4zSyMbFFlyzhrfYw
         axNn177pgUHVRMLD5rxlYpVfAinBTdWYynevo3ju9IjA8TaHMZ8mKEjN3C+VO6cfaN8m
         U+M4GKhih8Z7+NtAG8hlpR6yOuoS+RN45jHdWiNidV06Wersd6OyOXRpwydR/80gBErA
         7IC6dQ3IxHPBF4ibnoCmoVNlcsnoUXUlljcnskpxxUEqBmatKSOPgs5CANVOA22PEeGd
         O/7w==
X-Google-DKIM-Signature: v=1; a=rsa-sha256; c=relaxed/relaxed;
        d=1e100.net; s=20210112;
        h=x-gm-message-state:mime-version:from:date:message-id:subject:to;
        bh=ag9uumXWLjZpIPOuU+KsLUBIZ85kfwVB37B15cJYa+E=;
        b=hVRVJs6NXnQ3jYKm/D92GGtlWTiRWPw6H93JkW+7w160Mrz/y2PFx3B1GO0Xg3ftZC
         iFTx70RlgOexsdTPXsIHFO2mo8hE9FjHjo9g+uQ2Q2gslyMrr+3QmKbE5ZsnJZ0Bg22j
         orlNbecM5AMETX6SC2GPMbjNo49iVi+frK0aW+2IEIacYfaELQ/pJr1k5hmnjgGFoaXa
         SXd9s9c4ZkGzTyAZt/3M9tqrPxf3fOknvpLsRlWmfqVxBnoFI9cSWGo0C/kBYLLatnLU
         8eoYvv7TOxE8dcUCmIv7+M8CyJc8/vRegHDeq1DnltDtY3Ao4pUWs7zb5zO6FeivQY/D
         46tw==
X-Gm-Message-State: AOAM531gzA3/cMjCEjFdg1DdJwQgz5G7AdKRnH2YMlPAQSBz1DhQLcV8
	vzHxze7nJs2zQJSrnC1/x9SPBS083usA02OYfK2OIafylwE=
X-Google-Smtp-Source: ABdhPJy0Cez9JEYC0zkKVutY8WInkhlxN/w4OYUY8cqXK5POs+S5TrRIw4i5UBmKYYkVJqXichxESK0XeiSIq7U9vbo=
X-Received: by 2002:a05:6214:252d:b0:441:58fc:9b99 with SMTP id
 gg13-20020a056214252d00b0044158fc9b99mr11199898qvb.92.1650367589547; Tue, 19
 Apr 2022 04:26:29 -0700 (PDT)
MIME-Version: 1.0
From: =?UTF-8?B?VHLGsMahbmcgUXXhu5FjIEtow6FuaA==?= <tqk2811@gmail.com>
Date: Tue, 19 Apr 2022 18:26:18 +0700
Message-ID: <CAPC_KRz_grHcwRQWSZTNoOtVXP98CSEBb1yXimGZwmCjz3_DKw@mail.gmail.com>
Subject: test send email
To: qscski@candassociates.com
Content-Type: multipart/alternative; boundary=""000000000000ec826505dd0025f5\""


--000000000000ec826505dd0025f5Content-Type: text/plain; charset=""UTF-8""
Content-Transfer-Encoding: base64

dGVzdCBj4bq7w6JzZA0K
--000000000000ec826505dd0025f5
Content-Type: text/html; charset=""UTF-8""
Content-Transfer-Encoding: quoted-printable

<div dir=3D""ltr""> test c=E1=BA=BB=C3=A2sd<div><br></div></div>

--000000000000ec826505dd0025f5--
";

        public static void Test()
        {
            using MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(a));
            ms.Position = 0;
            using StreamReader streamReader = new StreamReader(ms);

            bool isLastEmpty = false;
            bool found = false;
            string boundary = string.Empty;
            while (!streamReader.EndOfStream && !found)
            {
                string line = streamReader.ReadLine();
                bool isCurrentEmpty = string.IsNullOrEmpty(line);
                if (isCurrentEmpty)
                {
                    if (isLastEmpty)
                    {
                        if (isCurrentEmpty) found = true;
                    }
                    else isLastEmpty = isCurrentEmpty;
                }
                else
                {
                    if (line.StartsWith("Content-Type"))
                    {
                        Match match = Regex.Match(line, "boundary=\\\"(.*?)\\\"");
                        if (match.Success) boundary = match.Groups[1].Value.TrimEnd('\\');
                    }
                }
            }
            if (found)
            {
                string body = streamReader.ReadToEnd();
                if (!string.IsNullOrEmpty(boundary))
                {
                    //fix boundary
                    body = Regex.Replace(body, $"{boundary}[^\\r-]", $"{boundary}C\r\n");
                }
                using MemoryStream ms2 = new MemoryStream(Encoding.UTF8.GetBytes(body));
                ms2.Position = 0;
                var p = HttpMultipartParser.MultipartFormDataParser.Parse(ms2);
                foreach (var file in p.Files)
                {
                    if (file.ContentType.Contains("text"))
                    {
                        using MemoryStream msFile = new MemoryStream();
                        file.Data.CopyTo(msFile);
                        string f_data = Encoding.UTF8.GetString(msFile.ToArray());
                    }
                }
            }
        }

#endif



        class EmailTm_ : IEmailTm
        {
            public string FromAddress { get; set; }

            public string Title { get; set; }

            public IEnumerable<string> Files { get { return Files_; } }
            internal List<string> Files_ { get; set; }
        }

        class Rootobject
        {
            public string context { get; set; }
            public string type { get; set; }
            public string id { get; set; }
            public string accountId { get; set; }
            public string msgid { get; set; }
            public From from { get; set; }
            public To[] to { get; set; }
            public string subject { get; set; }
            public string intro { get; set; }
            public bool seen { get; set; }
            public bool isDeleted { get; set; }
            public bool hasAttachments { get; set; }
            public string downloadUrl { get; set; }
            public int size { get; set; }
            public DateTime createdAt { get; set; }
            public DateTime updatedAt { get; set; }
        }

        class From
        {
            public string address { get; set; }
            public string name { get; set; }
        }

        class To
        {
            public string address { get; set; }
            public string name { get; set; }
        }








        class MailTmToken
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("token")]
            public string Token { get; set; }
        }

        class MailTmAccount
        {
            public string address { get; set; }
            public string password { get; set; }
        }


        class MailTmDomain
        {
            [JsonProperty("hydra:member")]
            public List<HydraMember> HydraMembers { get; set; }
        }

        class HydraMember
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("domain")]
            public string Domain { get; set; }

            [JsonProperty("isActive")]
            public bool IsActive { get; set; }

            [JsonProperty("isPrivate")]
            public bool IsPrivate { get; set; }
        }

    }
}
