namespace TqkLibrary.Net.Mails.TempMails.Wrapper
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMailWrapperEmail
    {
        /// <summary>
        /// 
        /// </summary>
        public string FromAddress { get; }
        /// <summary>
        /// 
        /// </summary>
        public string Subject { get; }
        /// <summary>
        /// 
        /// </summary>
        public string RawBody { get; }
    }
}
