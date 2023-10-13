using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TqkLibrary.Net.CloudStorage.Dropbox;

namespace TestProject.CloudStorage
{
    [TestClass]
    public class DropboxTest
    {
        readonly DropboxApiNonLogin _dropboxApiNonLogin = new DropboxApiNonLogin();
        public static IEnumerable<object[]> Folders
        {
            get
            {
                yield return new object[] { "https://www.dropbox.com/scl/fo/eozikq3mz4fvl954qf86i/h?rlkey=hasfub5ju614ytfvqwd4gqohp&dl=0" };
                //yield return new object[] { "https://www.dropbox.com/scl/fo/7metrzawj5yxaed6lo3pg/h?rlkey=4e52xxnviyr1h5chybqu6ah7z&dl=0" };
                //yield return new object[] { "https://www.dropbox.com/sh/rokm702oghrxxdv/AADEvmkDKbfoYisr_BEm2LjVa?dl=0" };
            }
        }
        public static IEnumerable<object[]> Files
        {
            get
            {
                yield return new object[] { "https://www.dropbox.com/scl/fi/0sskovcwdudgl97x2fuz1/Intro.mp4?rlkey=qxp0gw0wxzbs9bfedskderdwx&dl=0" };
            }
        }

        [TestMethod(), DynamicData(nameof(Files))]
        public async Task GetCookie(string folderUrl)
        {
            await _dropboxApiNonLogin.GetCookieAsync(folderUrl);
            Assert.IsFalse(string.IsNullOrWhiteSpace(_dropboxApiNonLogin.GetT()));
        }



        [TestMethod(), DynamicData(nameof(Folders))]
        public async Task ListChildInFolder(string folderUrl)
        {
            var urlInfo = await _dropboxApiNonLogin.ListPublicFolderAsync(folderUrl);
        }


        [TestMethod, DynamicData(nameof(Files))]
        public async Task GetMetaData(string fileUrl)
        {
            var metadata = await _dropboxApiNonLogin.GetMetadataAsync(fileUrl);
        }

        [TestMethod, DynamicData(nameof(Files))]
        public async Task Download(string fileUrl)
        {
            using Stream stream = await _dropboxApiNonLogin.DownloadFileAsync(fileUrl);
            using MemoryStream memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
        }
    }
}
