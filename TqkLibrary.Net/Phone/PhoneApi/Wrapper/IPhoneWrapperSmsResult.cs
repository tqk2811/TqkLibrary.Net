using System.Collections.Generic;

namespace TqkLibrary.Net.Phone.PhoneApi.Wrapper
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPhoneWrapperSmsResult<out T> : IEnumerable<T> where T : IPhoneWrapperSms
    {
        /// <summary>
        /// 
        /// </summary>
        public bool IsTimeout { get; }
    }
}
