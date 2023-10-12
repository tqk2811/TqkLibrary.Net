namespace TqkLibrary.Net.CloudStorage.OneDrive
{
    public partial class OneDriveApiNonLogin
    {
        /// <summary>
        /// 
        /// </summary>
        public class OneDriveLinkInfo
        {
            /// <summary>
            /// 
            /// </summary>
            public string ResourceId { get; internal set; }
            /// <summary>
            /// 
            /// </summary>
            public string AuthKey { get; internal set; }
            /// <summary>
            /// 
            /// </summary>
            public string E { get; internal set; }
            /// <summary>
            /// 
            /// </summary>
            public string Cid { get; internal set; }
            /// <summary>
            /// 
            /// </summary>
            public string ItHint { get; internal set; }
            /// <summary>
            /// f folder<br></br>
            /// v video<br></br>
            /// u unknow<br></br>
            /// </summary>
            public string Type { get; internal set; }
        }
    }
}