using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace TqkLibrary.Net.Mails.TempMails
{
    /// <summary>
    /// 
    /// </summary>
    public class TempMailOrg : BaseApi
    {
        /// <summary>
        /// 
        /// </summary>
        public TempMailOrg()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<TempMailOrgToken> InitToken(CancellationToken cancellationToken = default)
            => Build()
            .WithUrl("https://mob2.temp-mail.org/mailbox", HttpMethod.Post)
            .WithHeader("User-Agent", "3.00")
            .WithHeader("Accept", "application/json")
            .WithHeader("Host", "mob2.temp-mail.org")
            //.WithHeader("Connection", "Keep-Alive")
            .WithHeader("Accept-Encoding", "gzip, deflate")
            .WithCancellationToken(cancellationToken)
            .ExecuteAsync<TempMailOrgToken>();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<TempMailOrgMailBox> Messages(TempMailOrgToken token, CancellationToken cancellationToken = default)
            => Build()
            .WithUrlGet("https://mob2.temp-mail.org/messages")
            .WithHeader("Authorization", token.Token)
            .WithHeader("Host", "mob2.temp-mail.org")
            .WithHeader("User-Agent", "3.02")
            .WithCancellationToken(cancellationToken)
            .ExecuteAsync<TempMailOrgMailBox>();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<TempMailOrgMessageData> MessageData(TempMailOrgToken token, TempMailOrgMessageReView messageReView, CancellationToken cancellationToken = default)
            => Build()
            .WithUrlGet($"https://mob2.temp-mail.org/messages/{messageReView.Id}")
            .WithHeader("Authorization", token.Token)
            .WithHeader("Host", "mob2.temp-mail.org")
            .WithHeader("User-Agent", "3.02")
            .WithCancellationToken(cancellationToken)
            .ExecuteAsync<TempMailOrgMessageData>();

    }


#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class TempMailOrgToken
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("mailbox")]
        public string MailBox { get; set; }
    }

    public class TempMailOrgMailBox
    {
        [JsonProperty("mailbox")]
        public string MailBox { get; set; }

        [JsonProperty("messages")]
        public List<TempMailOrgMessageReView> Messages { get; set; }
    }

    public class TempMailOrgMessageReView
    {
        [JsonProperty("_id")]
        public string Id { get; set; }


        [JsonProperty("receivedAt")]
        public long ReceivedAt { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }


        [JsonProperty("subject")]
        public string Subject { get; set; }


        [JsonProperty("bodyPreview")]
        public string BodyPreview { get; set; }


        [JsonProperty("attachmentsCount")]
        public int AttachmentsCount { get; set; }
    }
    public class TempMailOrgMessageData : TempMailOrgMessageReView
    {
        [JsonProperty("user")]
        public string User { get; set; }
        
        [JsonProperty("mailbox")]
        public string Mailbox { get; set; }
        
        [JsonProperty("bodyHtml")]
        public string BodyHtml { get; set; }
        
        //public object[] attachments { get; set; }
        
        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }
    }

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
