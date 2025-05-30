using System;
namespace TqkLibrary.Net.Mail.Wrapper
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMailWrapperEmail
    {
        /// <summary>
        /// 
        /// </summary>
        string? FromAddress { get; }
        /// <summary>
        /// 
        /// </summary>
        string? Subject { get; }
        /// <summary>
        /// 
        /// </summary>
        string? RawBody { get; }
        
        /// <summary>
        /// 
        /// </summary>
        string? Code { get; }

        /// <summary>
        /// 
        /// </summary>
        DateTime? ReceivedTime { get; }
    }
}
